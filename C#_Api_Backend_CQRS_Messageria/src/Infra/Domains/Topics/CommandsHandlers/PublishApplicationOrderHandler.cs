using Core.Domains.Ordering.Models;
using Core.Models.Core.Customers;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Core.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Models.Core.Ordering;
using System.Linq.Dynamic.Core;
using Utils.Extensions;
using Core.Models.Core.Payments;
using Core.Models.Core.Geography;

namespace Infra.QueryCommands.Commands.Topics
{
    class PublishApplicationOrderHandler : IRequestHandler<PublishApplicationOrder, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationOrderHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationOrderHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationOrderHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationOrder request, CancellationToken cancellationToken)
        {
            try
            {
                var orderPub = new List<PublishApplicationOrder>();
                var orderList = this._context.Orders.Where<Order>(x => x.StatusId == OrderStatus.Paid.Id).ToList();

                foreach (Order order in orderList)
                {
                    try
                    {
                        order.StatusId = OrderStatus.Integrated.Id;
                        order.Items = this._context.OrderItems.Where<OrderItem>(x => x.OrderId == order.Id).ToList();
                        order.Customer = this._context.Customers.FirstOrDefault<Customer>(x => x.Id == order.CustomerId);
                        order.Seller = this._context.Sellers.FirstOrDefault<Seller>(x => x.Id == order.SellerId);
                        order.Shipping = this._context.OrderShippings.FirstOrDefault<OrderShipping>(x => x.Id == order.ShippingId);
                        order.Shipping.Address = this._context.Addresses.FirstOrDefault<Address>(x => x.Id == order.Shipping.AddressId);
                        order.Payment = this._context.Payments.FirstOrDefault<Payment>(x => x.Id == order.PaymentId);

                        if (order.PaymentId != null)
                        {
                            order.Payment.Transactions = this._context.PaymentTransaction.Where<PaymentTransaction>(x => x.PaymentId == order.PaymentId).ToList();
                        }

                        orderPub.Add(ConvertOrderDBtoDTO(order));
                    }
                    catch (Exception ex)
                    {
                        order.StatusId = OrderStatus.IntegrationFailed.Id;
                        await _context.SaveChangesAsync();
                    }
                }

                return new Response("Sucess", false, orderPub);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, true, new List<PublishApplicationOrder>());
            }
        }

        PublishApplicationOrder ConvertOrderDBtoDTO(Order order)
        {
            var pubOrder = new PublishApplicationOrder
            {
                brandId = this._identity.CurrentCompanyName.ToString(),
                channelType = "ECOMMERCE",
                companyId = this._identity.CurrentCompany.ToString(),
                orderId = order.Id.ToString(),
                paymentStatus = (order.Payment != null) && (order.Payment.Status == Core.Models.Core.Payments.PaymentStatus.Processed) ? "COMPLETE" : "DUE",
                placedAt = DateTimeExtensions.ToDateTimeUTCFormat(order.OrderedAt.ToLocalTime()),
                pointOfSaleId = order.StoreCode.Trim(),
                status = "PROCESSING",
                total = StringExtensions.ToDecimal(order.Value.ToString()),
                device = "MIXED",
                discount = StringExtensions.ToDecimal(order.Discount.ToString()),
                attributes = new List<PublishApplicationOrder_Attributes>
                {
                    new PublishApplicationOrder_Attributes
                    {
                        id = "CANAL",
                        description = "CANAL",
                        value = "SMARTSALES"
                    }
                },
                fulfillments = new List<PublishApplicationOrder_Fulfillment>(),
                items = new List<PublishApplicationOrder_Item>()
            };

            if ((order.Shipping != null) && (order.Shipping.Address != null))
            {
                pubOrder.billingAddress = new PublishApplicationOrder_BillingAddress
                {
                    addressName = "Residencial",
                    latitude = 0,
                    longitude = 0,
                    notes = "Entregar em horário comercial",
                    city = order.Shipping.Address.CityName,
                    addressId = order.Shipping.AddressId.ToString(),
                    addressLine1 = order.Shipping.Address.StreetName,
                    addressLine2 = order.Shipping.Address.Complement,
                    number = order.Shipping.Address.StreetNumber,
                    neighbourhood = order.Shipping.Address.DistrictName ?? "",
                    zipCode = order.Shipping.Address.ZipCode,
                    state = order.Shipping.Address.StateName,
                    country = "BR",
                    @default = true
                };

                if (order.Customer != null)
                {
                    pubOrder.billingAddress.contactName = order.Customer.Name;
                    pubOrder.billingAddress.contactPhone = (order.Customer?.Phone).IsNullOrEmpty() ? "11111111111" : order.Customer?.Phone.RemoveDDIPhone();
                }
            }

            if (order.Customer != null)
            {
                pubOrder.customer = new PublishApplicationOrder_Customer
                {
                    acceptsMarketing = true,
                    customerId = order.CustomerId.ToString(),
                    createdAt = DateTimeExtensions.ToDateTimeUTCFormat(((DateTime)order.Customer.CreatedAt).ToLocalTime()),
                    dateOfBirth = DateTimeExtensions.ToDateTimeUTCFormat((((DateTime)order.Customer.CreatedAt).AddYears(-20)).ToLocalTime()).Substring(0, 10)
                };

                if (order.Customer.BirthDay != null)
                {
                    pubOrder.customer.dateOfBirth = DateTimeExtensions.ToDateTimeUTCFormat(((DateTime)order.Customer.BirthDay).ToLocalTime());
                }

                pubOrder.customer.documentNumber = new PublishApplicationOrder_DocumentNumber
                {
                    cpf = order.Customer.Cpf != null ? StringExtensions.DigitsOnly(order.Customer.Cpf) : "",
                    rg = (order.Customer?.Rg).IsNullOrEmpty() ? "0000" : StringExtensions.DigitsOnly(order.Customer.Rg)
                };

                pubOrder.customer.email = order.Customer.Email;
                pubOrder.customer.firstName = StringExtensions.GetFirstName(order.Customer.Name);
                pubOrder.customer.gender = (order.Customer.Gender == null) ? "NOT INFORMED" : order.Customer.Gender;
                pubOrder.customer.lastName = StringExtensions.GetLastName(order.Customer.Name);
                pubOrder.customer.mobilePhone = (order.Customer?.Phone).IsNullOrEmpty() ? "11111111111" : order.Customer?.Phone.RemoveDDIPhone();
                pubOrder.customer.notes = "";
                pubOrder.customer.type = "PERSON";
                pubOrder.customer.updatedAt = DateTimeExtensions.ToDateTimeUTCFormat(((DateTime)order.Customer.ModifiedAt).ToLocalTime());
            }

            if (order.SellerId != null)
            {
                var seller = this._context.Sellers.FirstOrDefault(x => x.Id == order.SellerId);
                if (seller != null)
                {
                    pubOrder.salesAgent = new PublishApplicationOrder_SalesAgent()
                    {
                        amount = 0,
                        id = seller.OriginId.ToString(),
                        name = seller.Name,
                        documentNumber = new PublishApplicationOrder_DocumentNumber()
                        {
                            cpf = seller.Document.IsNullOrEmpty() ? "12345678909" : seller.Document,
                            rg = "0000"
                        }
                    };

                    pubOrder.salesAgent.amount = 0;
                }
            }

            var fulfillment = new PublishApplicationOrder_Fulfillment
            {
                items = new List<PublishApplicationOrder_FullItem>(),
                status = "WAITING",
                fulfillmentId = order.ShippingId.ToString(),
                locationId = order.StoreCode.Trim(),
                scheduledDeliveryPeriod = "Horário Comercial",
                scheduledDeliveryDate = DateTimeExtensions.ToDateTimeUTCFormat((((DateTime)order.Customer.CreatedAt).AddDays(5)).ToLocalTime()),
                scheduledDeliveryHours = new PublishApplicationOrder_ScheduledDeliveryHours
                {
                    from = "08:00:00Z",
                    to = "18:00:00Z"
                }
            };

            if (order.OrderTypeId == OrderType.Pickup.Id)
            {
                fulfillment.type = "PICKUP";
                fulfillment.pickup = new PublishApplicationOrder_FullfimentPickup()
                {
                    amount = StringExtensions.ToDecimal(order.ShippingAmmount.ToString()),
                    daysToPickup = order.Shipping.DaysToDelivery,
                    type = "STANDARD",
                    personToPickup = new PublishApplicationOrder_FullfimentPickupPeson()
                    {
                        documentNumber = pubOrder.customer.documentNumber.cpf,
                        documentType = "CPF",
                        name = pubOrder.customer.firstName
                    }
                };
            }
            else
            {

                fulfillment.type = "SHIPMENT";
                fulfillment.shipment = new PublishApplicationOrder_Shipment
                {
                    type = "SHIPMENT_BY_STORE",
                    carrier = "Correios",
                    method = order.Shipping.Name,
                    amount = StringExtensions.ToDecimal(order.ShippingAmmount.ToString()),
                    cost = StringExtensions.ToDecimal(order.ShippingAmmount.ToString()),
                    deliveryDate = DateTimeExtensions.ToDateTimeUTCFormat((order.OrderedAt.Date).AddDays(order.Shipping?.DaysToDelivery ?? 1)),
                    daysToDelivery = order.Shipping.DaysToDelivery,
                    discount = 0,
                    address = new PublishApplicationOrder_Address
                    {
                        country = "BR",
                        latitude = 0,
                        longitude = 0,
                        notes = "",
                        addressId = order.Shipping.AddressId.ToString(),
                        addressLine1 = order.Shipping.Address.StreetName,
                        addressLine2 = order.Shipping.Address.Complement,
                        addressName = "Principal",
                        city = order.Shipping.Address.CityName,
                        contactName = pubOrder.customer.firstName,
                        contactPhone = pubOrder.customer.mobilePhone,
                        neighbourhood = order.Shipping.Address.DistrictName,
                        number = order.Shipping.Address.StreetNumber,
                        state = order.Shipping.Address.StateName,
                        zipCode = order.Shipping.Address.ZipCode,
                        @default = true
                    }
                };
            }

            foreach (OrderItem item in order.Items)
            {
                var pubItem = new PublishApplicationOrder_Item
                {
                    quantity = item.Units,
                    discount = item.UnitDiscount != null ? StringExtensions.ToDecimal(item.UnitDiscount.ToString()) : 0,
                    giftPackage = false,
                    giftProduct = false,
                    price = StringExtensions.ToDecimal(item.UnitPrice.ToString()),
                    subTotal = StringExtensions.ToDecimal((item.UnitPrice * item.Units).ToString()),
                    sku = new PublishApplicationOrder_Sku
                    {
                        sku = item.StockKeepingUnit,
                        skuId = item.StockKeepingUnit,
                        weight = new PublishApplicationOrder_Weight
                        {
                            unit = "kg",
                            value = 0
                        },
                        width = new PublishApplicationOrder_Width
                        {
                            unit = "cm",
                            value = 0
                        },
                        height = new PublishApplicationOrder_Height
                        {
                            unit = "cm",
                            value = 0
                        },
                        length = new PublishApplicationOrder_Length
                        {
                            unit = "cm",
                            value = 0
                        }
                    },
                };
                pubOrder.items.Add(pubItem);

                var fulItem = new PublishApplicationOrder_FullItem
                {
                    name = item.ProductName,
                    quantity = item.Units,
                    skuId = item.StockKeepingUnit
                };
                fulfillment.items.Add(fulItem);
            }

            pubOrder.fulfillments.Add(fulfillment);

            var payment = new PublishApplicationOrder_Payment
            {
                paymentId = order.OrderCode.ToString(),
                status = "PAID",
                currency = "BRL",
                balance = new PublishApplicationOrder_Balance()
                {
                    paid = new PublishApplicationOrder_Paid()
                }
            };

            if (order.Payment != null)
            {
                payment.balance.due = new PublishApplicationOrder_Due
                {
                    amount = 0,
                    applicationId = this._identity.LinxIOApplicationId
                };

                payment.balance.paid.amount = order.Payment.Amount > 0 ? (order.Payment.Amount / 100) : 0;
                payment.balance.paid.applicationId = this._identity.LinxIOApplicationId;
                payment.balance.paid.date = DateTimeExtensions.ToDateTimeUTCFormat(order.OrderedAt);

                payment.details = new PublishApplicationOrder_PaymentDetails
                {
                    provider = "PAGARME"
                };

                foreach (PaymentTransaction trans in order.Payment.Transactions)
                {
                    if (order.Payment.PaymentTypeId == 1)// Cartão de Crédito
                    {
                        payment.details.creditCard = new PublishApplicationOrder_CreditCard
                        {
                            acquirer = "18727053000174",
                            authorizationDate = DateTimeExtensions.ToDateTimeUTCFormat((DateTime)trans.CreatedAt),
                            authorizationNumber = trans.AuthorizationCode.ToString(),
                            capturedDate = DateTimeExtensions.ToDateTimeUTCFormat((DateTime)trans.CreatedAt),
                            installmentAmount = trans.Ammount > 0 ? (trans.Ammount / 100) : 0, // valor da parcela
                            installments = (int)order.Payment.Installments,
                            issuer = new PublishApplicationOrder_Issuer
                            {
                                standard = CardBrandTypeId(order.Payment.CardBrandTypeId)
                            },
                            interestTotal = 0,
                            nsu = trans.Nsu,
                            numberBin = String.IsNullOrEmpty(order.Payment.Pan) ? "000000" : order.Payment.Pan.Substring(0, 6)
                        };

                        payment.details.type = "CREDITCARD";
                    }
                    else if (order.Payment.PaymentTypeId == 2)// Cartão de Débito
                    {
                        payment.details.debitCard = new PublishApplicationOrder_DebitCard
                        {
                            acquirer = "18727053000174",
                            authorizationNumber = trans.AuthorizationCode.ToString(),
                            issuer = new PublishApplicationOrder_Issuer
                            {
                                standard = CardBrandTypeId(order.Payment.CardBrandTypeId)
                            },
                            nsu = trans.Nsu
                        };
                        payment.details.type = "DEBITCARD";
                    }
                    else if (order.Payment.PaymentTypeId == 4)// Pix
                    {
                        payment.details.pix = new PublishApplicationOrder_Pix
                        {
                            acquirer = "18727053000174",
                            bankCode = "",
                            transactionId = trans.Id.ToString()
                        };
                        payment.details.type = "PIX";
                    }
                }
            }

            pubOrder.payments = new List<PublishApplicationOrder_Payment>
            {
                payment
            };

            return pubOrder;
        }

        private string CardBrandTypeId(int? id)
        {
            if (id == null) return "VISA";
            var result = "VISA";

            switch (id)
            {
                case 1:
                    result = "VISA";
                    break;
                case 2:
                    result = "MASTER";
                    break;
                case 3:
                    result = "AMEX";
                    break;
                case 4:
                    result = "JCB";
                    break;
                case 5:
                    result = "DISCOVER";
                    break;
                case 6:
                    result = "ELO";
                    break;
            }

            return result;
        }
    }
}
