using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Core.Domains.Ordering.Models;
using Core.Models.Core.Customers;
using Core.Models.Core.Ordering;
using Core.Models.Core.Payments;
using Core.Repositories;

using Infra.ExternalServices.Authentication;
using Infra.ExternalServices.Fiscal.Dtos.PagarMe;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Utils;
using Utils.Extensions;

namespace Infra.ExternalServices.Fiscal
{

    public class PagarMeOrderIntegrationService : IPagarMeOrderIntegrationService
    {
        private readonly ILogger<PagarMeOrderIntegrationService> _logger;
        private readonly IAuthenticationService _authenticationServices;
        private readonly IStoreRepository _storeRepository;
        private readonly SmartSalesIdentity _identity;
        
        public PagarMeOrderIntegrationService(ILogger<PagarMeOrderIntegrationService> logger,
                                             IAuthenticationService authenticationServices, 
                                             SmartSalesIdentity identity, 
                                             IStoreRepository storeRepository) {
            
            this._authenticationServices = authenticationServices;
            this._storeRepository = storeRepository;
            this._identity = identity;
            this._logger = logger;
        }

        public async Task<bool> CreateOrder(Order order, 
                                                   List<OrderItem> items, 
                                                   OrderShipping shipping, 
                                                   Payment payment, 
                                                   Customer customer) { 
            
            var data = new PagarMeOrder {
                Id = order.GetReference(),
                
                ClientId = "smartsales",
                ChannelId = "smartsales",
                PointOfSaleId = "smartsales",
                Source    = "smart sales",
                Status    = nameof (OrderStatus.Paid),
                
                Total     = order.GetNetTotal(),
                Discount  = order.GetDiscounts(),
                
                PlacedAt  = order.CreatedAt ?? DateTime.Now,
                CreatedAt = order.CreatedAt ?? DateTime.Now,
                UpdatedAt = order.CreatedAt ?? DateTime.Now,
                
                BillingAddress = MakeAddress(shipping, customer),
                
                Customer = new PagarMeOrderCustomer{
                    Id = customer.Id.ToString(),
                    FirstName = customer.Name,
                    FullName = customer.Name,
                    LastName = "",
                    Email = customer.Email,
                    Gender = customer.Gender,
                    SocialNumber = customer.Phone,
                    Type = "INDIVIDUAL",
                    Documents = new [] {
                        new PagarMeDocument {
                            Number = customer.Cpf,
                            Type = "cpf"
                        },
                        new PagarMeDocument {
                            Number = customer.Rg,
                            Type   = "rg"
                        },
                    },
                    Telephones = new [] {
                        new PagarMeTelephone{
                            Number      = customer.Phone,
                            Description = "Principal",
                            Type        = "billing",
                            CountryCode = ""
                        }
                    },
                    Addresses = new PagarMeAddresses {
                        Billing = MakeAddress(shipping, customer)
                    }
                },
                
                Items = MakeItems(items),
                
                PagarMeTotals = MakeTotals(order),
                
                Payments = new []{
                    MakePayment(payment)
                },
                
                Fulfillments = new Dictionary<string, PagarMeFulllfilment>{
                    {"01", new PagarMeFulllfilment{
                        Id = "01",
                        OrderId   = order.GetReference(),
                        Ownership = "Linx SmartSales",
                        ChannelId = "smartsales",
                        ClientId  = "smartsales",
                        Status    = "PROCESSED",
                        Type      = (OrderTypeEnum) order.OrderTypeId == OrderTypeEnum.Pickup ? "PICKUP" : "SHIPMENT",
                        Presale   = true,
                        BillingDeadlineAt = DateTime.Now,
                        DeadlineAt = DateTime.Now,
                        CreatedAt = order.OrderedAt,
                        UpdatedAt = order.ModifiedAt ?? order.OrderedAt,
                        EnableBilling = false,
                        EnablePicking = true,
                        EnableShipping = true,
                        
                        ShippingAmount = 0, // order.ShippingAmmount,
                        
                        Totals = MakeTotals(order),
                        
                        Items = MakeItems(items).ToDictionary(key => key.Sku, value => value),
                        
                        Shipment = new PagarMeShipment{
                            Method = "",
                            Address = MakeAddress(shipping, customer),
                        },
                        FreightCosts = new PagarMeFreightCosts{
                            TotalTime = 0,
                            HandlingPrice = 0,
                            HandlingTime = 0,
                            TotalPrice = 0
                        },
                        Volumes = new [] {
                            new PagarMeVolumes{
                                Height = 0,
                                Length = 0,
                                Volume = 0,
                                Quantity = 0,
                                Weight = 0,
                                Width = 0,
                                
                                Items = MakeItems(items),
                                
                            } 
                        },
                        PrePickingFinished = false,
                        EnablePrePicking = false,
                        LocationType = "Own Store",
                        LocationId = this._identity.CurrentStoreCode,
                        ProcessedAt = order.OrderedAt,
                        ReservationId = "",
                        StatusHistory = new [] {
                            new PagarMeStatusHistory{
                                Status = "PROCESSED",
                                CreatedAt = order.CreatedAt,
                                ProcessedAt = order.CreatedAt,
                                TimeInStatus = 0,
                                State = "PROCESSED",
                            }
                        }
                    }},
                }
            };

            return await SendPayload(data);
        }
        
        private static PagarMePayments MakePayment(Payment payment)
        {
            string paymentType = "";
            string brandName = "";
            switch ((PaymentTypeEnum) payment.PaymentTypeId)
            {
                case PaymentTypeEnum.CreditCard:
                    paymentType = "CREDIT_CARD";
                    brandName = payment.GetCardBrand().ToString();
                    break;
                case PaymentTypeEnum.DebitCard:
                    paymentType = "DEBIT_CARD";
                    brandName = payment.GetCardBrand().ToString();
                    break;
                case PaymentTypeEnum.Pix:
                    paymentType = "PIX";
                    brandName = "UNKNOWN";
                    break;
                default:
                    paymentType = "UNKNOWN";
                    brandName = "UNKNOWN";
                    break;
            }
            var payments = new PagarMePayments{
                Amount            = (payment.Amount / 100.0),
                Currency          = payment.Currency,
                Brand             = brandName,
                Installments      = (int) payment.Installments,
                Type              = paymentType,
                TransactionNumber = payment.Transactions.FirstOrDefault(x => x.IsSuccess)?.Nsu,
                BillingDate       = payment.Transactions.FirstOrDefault(x => x.IsSuccess)?.CreatedAt ?? DateTime.Now,
                IsCaptured        = payment.Transactions.FirstOrDefault(x => x.IsSuccess)?.IsSuccess ?? false,
                PaymentNumber     = 0
            };
            
            return payments;
        }
        
        public static PagarMeItem[] MakeItems(List<OrderItem> items)
        {
            return items.Select(orderItem => new PagarMeItem{
                Sku   = orderItem.ProductVariation.StockKeepingUnit.Replace("-", ""),
                Name  = orderItem.ProductName,
                Url   = "",
                Gtin  = "",
                Image = orderItem.ProductVariation.ImageUrl,

                Price = orderItem.GetNetValue(),

                Discount        = orderItem.UnitDiscount ?? 0,
                BasePrice       = orderItem.GetGrossValue(),
                Quantity        = orderItem.Units,
                AcquisitionType = "PURCHASE",
                Height          = 0,
                Weight          = 0,
                Width           = 0,
                Length          = 0,
                
                StockType        = "PHYSICAL",
                OrderedQuantity  = orderItem.Units,
                ReturnedQuantity = 0,
                CanceledQuantity = 0,
                ItemType         = "OTHER",
                ShippingPrice    = 0
            }).ToArray();
        }
        
        
        public static PagarMeTotals MakeTotals(Order order)
        {
            return new PagarMeTotals {
                TotalAmount          = order.GetNetTotal(),
                Price                = order.GetGrossTotal(),
                PriceDiscounted      = order.GetDiscounts(),
                ShippingAmount       = 0, // order.ShippingAmmount,
                ProportionalDiscount = 0,
                GiftPackagePrice     = 0,
                PriceShippingAmount  = 0,
            };
        }
        
        public static PagarMeBillingAddress MakeAddress(OrderShipping shipping, Customer customer)
        {
            return new PagarMeBillingAddress{
                Address1        = shipping.Address.StreetName,
                Address2        = shipping.Address.Complement.Truncate(20),
                Zip             = shipping.Address.ZipCode,
                City            = shipping.Address.CityName,
                State           = shipping.Address.StateName,
                Country         = shipping.Address.CountryName,
                Number          = shipping.Address.StreetNumber,
                Neighbourhood   = shipping.Address.DistrictName,
                FirstName       = customer.Name,
                LastName        = "",
                Description     = "shipping",
                DefaultBilling  = false,
                DefaultShipping = false,
                Telephone = new PagarMeTelephone{
                    Number      = customer.Phone,
                    Description = "Principal",
                    Type        = "billing",
                    CountryCode = ""
                }
            };
        }
        
        private async Task<bool> SendPayload(PagarMeOrder config)
        {
            using (var client = new HttpClient()) {
                
                var requestBody = JsonConvert.SerializeObject(config, JsonSettings.Settings);

                var responseString = string.Empty;

                var storeErpSettings = await _storeRepository.GetStoreErpSettingsAsync(this._identity.CurrentStoreId ?? Guid.Empty);
                if (storeErpSettings == null) {
                    throw new Exception("Não foram encontradas informações de configuração para a loja");
                }
                
                var user = storeErpSettings.User;
                var password = storeErpSettings.Password;
                var host = storeErpSettings.ServiceHost;
                var environment = storeErpSettings.Environment;
                
                try {
                    var auth = await this._authenticationServices.AuthenticateAsync(user, password);

                    client.BaseAddress = new Uri(host);

                    client.DefaultRequestHeaders.TryAddWithoutValidation("EconomicGroup", auth.EconomicGroup);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("CurrentUser", auth.CurrentUser);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("AuthorizationToken", auth.AuthorizationToken);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("CurrentCompany", auth.CurrentCompany);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("AccessGroup", auth.AccessGroup);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Application", auth.Application);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Environment", auth.Environment);

                    var request = new HttpRequestMessage(HttpMethod.Post, $"{environment}/LinxOperacionalPedidoLojaPedido/CriaPedidoOmsNoUX");
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    var response = await client.SendAsync(request);

                    responseString = await response.Content.ReadAsStringAsync();
                    this._logger.LogInformation("--- Response from UX", responseString);

                    response.EnsureSuccessStatusCode();
                    
                    return true;
                    
                }  catch (HttpRequestException exception) {
                    
                    this._logger.LogCritical("Error <strong>{responseString}</strong> <br/> <pre>{body}</pre> <br/> <pre>{@error}</pre>", responseString, requestBody, exception);
                    
                    return false;
                    
                } finally {
                    
                    client.Dispose();
                }
            }
        }

    }

}