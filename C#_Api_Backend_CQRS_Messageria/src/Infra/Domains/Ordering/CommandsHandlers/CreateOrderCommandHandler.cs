using Core.Domains.Ordering.Models;
using Core.Models.Core.Customers;
using Core.Models.Core.Geography;
using Core.Models.Core.Ordering;
using Core.Models.Core.Payments;
using Core.Models.Core.Products;
using Core.SharedKernel;
using Core.SeedWork;
using CoreService.Infrastructure.Services;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Infra.ExternalServices.Payments.Contracts;
using Infra.ExternalServices.Payments.Dtos;
using Infra.ExternalServices.Reshop.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils.Extensions;
using Infra.Extensions;
using Core.Repositories;

namespace Infra.QueryCommands.Commands.Orders
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<CreateOrderCommandHandler> _logger;
        private readonly SmartSalesIdentity _identity;
        private readonly IPaymentGatewayServices _gateway;
        private readonly IStockRepository _stock;
        private readonly ICampaignIntegrationService _campaignService;
        private readonly ICatalogCache _catalogResponseCache;
        private readonly IStoreRepository _store;

        public CreateOrderCommandHandler(CoreContext context, ILogger<CreateOrderCommandHandler> logger, SmartSalesIdentity identity,
            IPaymentGatewayServices gateway, IStockRepository stock, ICatalogCache catalogResponseCache, ICampaignIntegrationService campaignService, IStoreRepository store)
        {
            _context = context;
            _logger = logger;
            _identity = identity;
            _gateway = gateway;
            _stock = stock;
            _catalogResponseCache = catalogResponseCache;
            _store = store;
        }

        public async Task<Customer> GetCustomer(CreateOrderCommand request)
        {
            var customer = this._context.Customers.FirstOrDefault(x => x.Cpf.Contains(request.User.Cpf));
            if (customer == null)
            {
                customer = new Customer
                {
                    Name = request.User.Name,
                    Email = request.User.Email,
                    Cpf = request.User.Cpf,
                    Rg = request.User.Rg,
                    Phone = request.User.Phone,
                    BirthDay = request.User.BirthDay,
                };

                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
            }
            else
            {
                customer.Name = request.User.Name ?? customer.Name;
                customer.Email = request.User.Email ?? customer.Email;
                customer.Rg = request.User.Rg ?? customer.Rg;
                customer.Phone = request.User.Phone ?? customer.Phone;
                customer.BirthDay = request.User.BirthDay ?? customer.BirthDay;

                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }

            return customer;
        }

        public async Task<ServiceResponse<Payment>> CreatePayment(Order order, List<OrderItem> orderItems, OrderShipping shipping, PaymentCardDto cardPayment, Customer customer, PaymentTypeEnum paymentTypeId)
        {
            ServiceResponse<Payment> serviceResponse = new ServiceResponse<Payment>();
            var response_pair = await _gateway.CreateOrder(order, orderItems, shipping, cardPayment, customer, paymentTypeId);
            var response = response_pair.Value;

            if (response.IsSuccess == false)
            {
                order.StatusId = OrderStatus.PaymentFailed.Id;
                await this._context.SaveChangesAsync();

                throw new Exception($"Não foi possível processar pagamento, {response.Message}");
            }

            // Updating OrderCode in order
            order.OrderCode = response_pair.Key;
            await this._context.SaveChangesAsync();

            var paymentResponse = response.ResponseData;
            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = paymentResponse.Amount,
                Currency = paymentResponse.Currency,
                Installments = paymentResponse.Installments,
                Pan = cardPayment == null ? null : cardPayment.Number.Substring(0, 4) + "********" + cardPayment.Number.Substring(cardPayment.Number.Length - 4),
                GatewayProvider = paymentResponse.GatewayProvider,
                StatusId = Enumeration.Cast<PaymentStatus>(paymentResponse.Status).Id,
                PaymentTypeId = (int)paymentTypeId,
                Transactions = paymentResponse?.Transactions?.Select(dto => new PaymentTransaction
                {
                    Ammount = dto.Ammount,
                    Country = dto.Country,
                    Currency = dto.Currency,
                    Installments = dto.Installments,
                    Nsu = dto.Nsu,
                    Suplier = dto.Suplier,
                    AuthorizationCode = dto.AuthorizationCode,
                    IsSuccess = dto.IsSuccess,
                    SuplierReturnCode = dto.SuplierReturnCode,
                    StatusId = Enumeration.Cast<PaymentTransactionStatus>(dto.Status).Id
                }).ToList()
            };

            payment.Amount = payment.Transactions?.Sum(x => x.Ammount) ?? (long)(order.Value * 1000);

            if (payment.StatusId == PaymentStatus.Failed.Id) order.StatusId = OrderStatus.PaymentFailed.Id;
            if (payment.StatusId == PaymentStatus.Processed.Id) order.StatusId = OrderStatus.Paid.Id;
            if (payment.StatusId == PaymentStatus.Processing.Id) order.StatusId = OrderStatus.Paid.Id;

            if (payment.PaymentTypeId == PaymentType.DebitCard.Id || payment.PaymentTypeId == PaymentType.CreditCard.Id)
                payment.CardBrandTypeId = (int)payment.GetCardBrand();

            serviceResponse.IsSuccess = true;
            serviceResponse.ResponseData = payment;
            serviceResponse.AuthenticationUrl = response.AuthenticationUrl;
            serviceResponse.QRCode = response.QRCode;
            serviceResponse.QRCodeUrl = response.QRCodeUrl;

            return serviceResponse;
        }

        public async Task ProccessCampaign(Order order, CreateOrderCommand request, PaymentCardDto cardPayment)
        {
            try
            {
                var codigoLoja = _identity.CurrentStoreCode;

                var processOperation = await this._campaignService.PostProcessCampaign(new ReshopProcessCampaign
                {
                    CodigoLoja = codigoLoja,
                    DataHora = DateTime.Now,
                    DataVenda = DateTime.Now,
                    TipoVenda = 1,
                    QtdeTotal = order.Items.Sum(x => x.Units),
                    ValorBruto = order.GetGrossTotal(),
                    ValorDescontoUnico = order.GetDiscounts(),
                    ValorLiquido = order.GetNetTotal(),
                    NumeroOperacao = order.GetReference(),
                    SenhaAutorizacao = "",
                    Nsu = request.Nsu,
                    TransacaoAssociada = request.TransacaoAssociada,
                    Result = request.Result ?? false,
                    Message = request.Message,
                    IsException = request.IsException ?? false,
                    Offline = request.Offline ?? false,
                    Itens = order.Items.Select(orderItem => new ItemProcessCampaign
                    {
                        Item = orderItem.Id.ToString(),
                        CodigoSku = orderItem.ProductVariation.StockKeepingUnit,
                        Qtde = orderItem.Units,
                        CodigoProduto = orderItem.Product.Reference,
                        PrecoUnitario = orderItem.GetUnitNetValue(),
                        ValorLiquido = orderItem.GetNetValue(),
                    }).ToList(),
                    Pagamentos = request.Pagamentos.Select(payments => new Pagamento
                    {
                        NumeroPagamento = payments.NumeroPagamento,
                        Vencimento = DateTime.Now,
                        Codigo = "PDSM-C",
                        Tipo = 3,
                        BinCartao = cardPayment == null ? "" : cardPayment.Number.Substring(0, 6),
                        Valor = order.GetNetTotal(),
                        Result = payments.Result,
                        Message = payments.Message
                    }).ToList()
                });

                var nsu = processOperation.Nsu;
                var saleNumber = "";

                await _campaignService.GetConfirmsCampaigns(codigoLoja, nsu, saleNumber, true);
            }
            catch (Exception e)
            {
                _logger.LogError("Erro {@error}", e);
            }
        }

        public async Task<Response> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            request.User.Phone = request.User.Phone.RemoveSpaces().DigitsOnly();
            if (!request.User.Phone.IsValidNumber())
                throw new Exception("Invalid phone number");

            if (request.Shipping == null)
                throw new Exception("Dados de entrega inválidos.");

            var card = request.Card;
            var user = request.User;
            var productsVariationsIds = request.Items.Select(x => x.ProductVariationId).ToList();

            var customer = await GetCustomer(request);

            var variations = await this._context.ProductVariations.Where(x => productsVariationsIds.Contains(x.Id)).Include(x => x.Product).ToListAsync();
            if (!variations.Any())
                throw new Exception("Produtos não encontrados");

            // isso muda o preço pegando do banco (caso algum engraçadinho mude isso no front-end)
            foreach (var item in request.Items)
            {
                var variation = variations.FirstOrDefault(y => y.Id == item.ProductVariationId) ?? new ProductVariation();
                item.Price = variation.BasePrice ?? throw new Exception("Valor de preço precisa ser preenchido no banco!");
            }

            var shippingAmmount = request.ShippingPrice;

            var catalog = request.CatalogId == null ? null : this._context.Catalogs.FirstOrDefault(x => x.Id == request.CatalogId);
            var seller = catalog == null ? null : this._context.Sellers.FirstOrDefault(x => x.Id == catalog.SellerId);
            seller = seller == null ? (request.SellerId == null ? null : this._context.Sellers.FirstOrDefault(x => x.Id == request.SellerId)) : seller;

            var order = new Order
            {
                Description = $"Pedido de {DateTime.Now:MM/dd/yyyy} as {DateTime.Now:HH:mm}",
                CatalogId = request.CatalogId,
                Catalog = catalog,
                StatusId = OrderStatus.Submitted.Id,
                CustomerId = customer.Id,
                Customer = customer,
                SellerId = seller?.Id,
                Seller = seller,
                ShippingAmmount = shippingAmmount,
                OrderTypeId = (int)request.OrderTypeId
            };

            this._context.Orders.Add(order);
            this._context.SaveChanges();

            if (order.CatalogId != null)
            {
                _catalogResponseCache.RemoveCatalogDetailsById(catalog.Id);
            }

            var orderItens = request.Items.Select(orderItem =>
            {
                var variant = variations.FirstOrDefault(y => y.Id == orderItem.ProductVariationId) ?? new ProductVariation();
                return new OrderItem
                {
                    ProductId = variant.Product.Id,
                    ProductVariationId = orderItem.ProductVariationId,
                    Units = orderItem.Quantity,
                    UnitPrice = variant.BasePrice ?? 0,
                    ProductName = variant.Product?.Name,
                    Description = variant.CompleteName ?? (variant.Name ?? (variant.Product?.Description ?? (variant.Product?.Name))),
                    UrlPicture = variant.ImageUrl,
                    StockKeepingUnit = variant.StockKeepingUnit,
                    Reference = variant.Product.Reference,
                    UnitDiscount = orderItem?.OrderDiscounts?.Sum(discount => discount.Value),
                    DiscountItems = orderItem?.OrderDiscounts?.Select(discount => new OrderItemDiscount
                    {
                        Value = discount.Value,
                        ReferenceId = discount.ReferenceId,
                        DiscountTypeId = Enumeration.Cast<OrderItemDiscountType>((int)discount.DiscountType).Id
                    }).ToList(),
                };
            }).ToList();

            PaymentCardDto cardPayment = null;
            ServiceResponse<Payment> paymentResponse = null;
            Payment payment = null;

            // Update stock to remove selected items if possible
            if (!this._stock.UpdateProductStock(orderItens))
                throw new Exception("Algum produto selecionado não está mais disponível em estoque.");

            try
            {
                var shipping = new OrderShipping
                {
                    OrderId = order.Id,
                    Price = order.ShippingAmmount,
                    TypeId = order.OrderTypeId,
                    ShippingId = request.Shipping.ShippingId,
                    Name = request.Shipping.ShippingName,
                    DaysToDelivery = request.Shipping.DaysToDelivery,
                    StoreIdPickup = request.Shipping.StoreIdPickup
                };

                if (shipping.TypeId == OrderType.Pickup.Id)
                {
                    var storeAddress = shipping.StoreIdPickup.HasValue ?
                        await _store.GetStoreAddressAsync(shipping.StoreIdPickup.Value) :
                        await _store.GetStoreAddressAsync(order.StoreCode);

                    shipping.Address = new Address
                    {
                        ZipCode = storeAddress.ZipCode,
                        StreetName = storeAddress.StreetName,
                        StreetNumber = storeAddress.StreetNumber,
                        DistrictName = storeAddress.DistrictName,
                        CityName = storeAddress.CityName,
                        StateName = storeAddress.StateName,
                        CountryName = storeAddress.CountryName,
                        Reference = storeAddress.DistrictName,
                        Complement = storeAddress.Description
                    };
                }
                else
                {
                    shipping.Address = new Address
                    {
                        ZipCode = request.Address.ZipCode,
                        StreetName = request.Address.StreetName,
                        StreetNumber = request.Address.Number,
                        DistrictName = request.Address.Reference,
                        CityName = request.Address.City,
                        StateName = request.Address.State,
                        CountryName = request.Address.Country,
                        Reference = request.Address.Reference,
                        Complement = request.Address.Complement,
                    };
                }

                order.Items = orderItens;
                order.Shipping = shipping;
                order.Value = order.GetNetTotal();
                order.Discount = order.GetDiscounts();
                order.Reference = order.GetReference();
                this._context.SaveChanges();

                var paymentType = Enumeration.Cast<PaymentType>(request.Pagamentos.FirstOrDefault().Tipo);
                if (card != null && ((paymentType.Id == (int)PaymentTypeEnum.CreditCard) || (paymentType.Id == (int)PaymentTypeEnum.DebitCard)))
                {
                    var validation = card.Validation.Split("/");
                    cardPayment = new PaymentCardDto
                    {
                        Installments = card.Installments == null ? (byte)1 : (byte)card.Installments,
                        Holder = card.Holder,
                        Number = card.Number,
                        SecurityCode = card.SecurityCode,
                        Month = validation[0],
                        Year = validation[1],
                    };
                }

                // Trecho que efetua o envio para o PagarMe
                paymentResponse = await CreatePayment(order, orderItens, shipping, cardPayment, customer, (PaymentTypeEnum)paymentType.Id);
                payment = paymentResponse.ResponseData;

                await _context.Payments.AddAsync(payment);
                order.PaymentId = payment.Id;
                order.Payment = payment;

                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                this._stock.UpdateProductStock(orderItens, true);
                throw exception;
            }

            //Send E-mail 
            //await ProccessCampaign(order, request, cardPayment);

            var history = order.Items.Select(x => new CustomersOrderHistory
            {
                Discount = x.UnitDiscount,
                Units = x.Units,
                CustomerId = customer.Id,
                StockKeepingUnit = x.StockKeepingUnit,
                GrossValue = x.GetGrossValue(),
                NetValue = x.GetNetValue(),
                OrderOrigin = "SMSL",
                OrderDate = order.OrderedAt
            }).ToList();

            await _context.OrderHistories.AddRangeAsync(history);

            if (catalog != null)
            {
                catalog.Revenues = null;
                catalog.NumOfSales = null;
            }

            customer.StoreCode = order.StoreCode;
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            if (paymentResponse?.AuthenticationUrl != null && paymentResponse?.AuthenticationUrl?.Length > 0) return new Response(new OrderResponse(order.Id, order.Reference, paymentResponse.AuthenticationUrl));
            else if (paymentResponse?.QRCode != null && paymentResponse?.QRCode?.Length > 0) return new Response(new OrderResponse(order.Id, order.Reference, paymentResponse.QRCode, paymentResponse.QRCodeUrl));
            else return new Response(new OrderResponse(order.Id, order.Reference));
        }
    }
}
