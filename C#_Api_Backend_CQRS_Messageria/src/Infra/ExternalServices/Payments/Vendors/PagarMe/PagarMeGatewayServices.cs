using System;
using System.Linq;
using Utils.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domains.Ordering.Models;
using Core.Models.Core.Customers;
using Core.Models.Core.Ordering;
using Core.Models.Core.Payments;
using Core.Repositories;
using Core.SharedKernel;
using Infra.ExternalServices.Authentication;
using Infra.ExternalServices.Payments.Contracts;
using Infra.ExternalServices.Payments.Dtos;
using Microsoft.Extensions.Logging;
using PagarMeApi.Models;
using PagarMeApi;
using Utils;
using Newtonsoft.Json;

namespace Infra.ExternalServices.Payments.Vendors.PagarMe
{
    public class PagarMeGatewayServices : IPaymentGatewayServices
    {
        public GatewayProvider GetGatewayProvider() => GatewayProvider.PagarMe;

        private readonly ILogger<PagarMeGatewayServices> _logger;
        private readonly IStoreRepository _storeRepository;
        private readonly SmartSalesIdentity _identity;
        private readonly ICompanyRepository _companyRepository;
        private Dictionary<string, string> _payment_status_dict;
        private Dictionary<string, string> _transaction_status_dict;
        private LinxIOPagarMeClientConfig pagarMeConfig;

        public PagarMeGatewayServices(ILogger<PagarMeGatewayServices> logger, IStoreRepository storeRepository, ICompanyRepository companyRepository, SmartSalesIdentity identity)
        {
            this._logger = logger;
            this._logger.LogInformation("Create PagarMeGatewayServices");
            this._storeRepository = storeRepository;
            this._identity = identity;
            this._companyRepository = companyRepository;

            // Adding status dictionary for PagarMe Payment status string to Order conversion
            this._payment_status_dict = new Dictionary<string, string>();
            this._payment_status_dict.Add("paid", "Processed");
            this._payment_status_dict.Add("pending", "Processing");
            this._payment_status_dict.Add("processing", "Processing");
            this._payment_status_dict.Add("captured", "Processing");

            // Adding status dictionary for PagarMe Transaction status string to Order conversion
            this._transaction_status_dict = new Dictionary<string, string>();
            this._transaction_status_dict.Add("paid", "Authorized");
            this._transaction_status_dict.Add("pending", "Captured");
            this._transaction_status_dict.Add("processing", "Captured");
            this._transaction_status_dict.Add("captured", "Captured");
            this._transaction_status_dict.Add("waiting_payment", "Captured");

            var config = _companyRepository.GetCompanySettingsAsync((this._identity.CurrentCompany ?? Guid.Empty), Core.Models.Identity.Companies.CompanySettingsType.PagarMeClientConfig).Result;
            this.pagarMeConfig = JsonConvert.DeserializeObject<LinxIOPagarMeClientConfig>(config.Value, JsonSettings.Settings);
        }

        public string FixPaymentStatus(string status)
        {
            return this._payment_status_dict.FirstOrDefault(x => x.Key == status).Value;
        }
        public string FixTransactionStatus(string status)
        {
            return this._transaction_status_dict.FirstOrDefault(x => x.Key == status).Value;
        }

        public CreatePaymentRequest CreatePaymentPayload(Order order, CreateCardRequest payload_card, PaymentTypeEnum paymentType, PaymentCardDto paymentCardDto)
        {
            CreatePaymentRequest ret = null;

            var configStore = _storeRepository.GetStoreSettingsAsync(_storeRepository.GetStoreByStoreCodeAsync(order.StoreCode).Result.Id, Core.Models.Identity.Stores.StoreSettingsType.PagarMeConfig).Result;
            var pagarMeStoreConfig = JsonConvert.DeserializeObject<LinxIOPagarMeStoreConfig>(configStore.Value, JsonSettings.Settings);
            var Amount = "";

            switch (paymentType)
            {
                case PaymentTypeEnum.CreditCard:
                    Amount = pagarMeStoreConfig.CreditCard;
                    ret = new CreatePaymentRequest
                    {
                        PaymentMethod = "credit_card",
                        // Gathering Credit Card Information
                        CreditCard = new CreateCreditCardPaymentRequest
                        {
                            Installments = paymentCardDto.Installments,
                            StatementDescriptor = _identity.CurrentStorePortal.ToString().RemoveSpecialCharacters().Left(13),
                            Capture = (pagarMeStoreConfig.TransactionTypeCreditCard.Equals("auth_and_capture")),
                            Recurrence = false,
                            Card = payload_card,
                            OperationType = pagarMeStoreConfig.TransactionTypeCreditCard
                        }
                    };
                    break;
                case PaymentTypeEnum.DebitCard:
                    Amount = pagarMeStoreConfig.DebitCard;
                    ret = new CreatePaymentRequest
                    {
                        PaymentMethod = "debit_card",
                        // Gathering Debit Card Information
                        DebitCard = new CreateDebitCardPaymentRequest
                        {
                            Card = new CreateCardRequest
                            {
                                HolderName = "Tony Stark",
                                Number = "342793631858229",
                                ExpMonth = 1,
                                ExpYear = 18,
                                Cvv = "3531",
                            }
                        }
                    };
                    break;
                case PaymentTypeEnum.Pix:
                    Amount = pagarMeStoreConfig.Pix;
                    ret = new CreatePaymentRequest
                    {
                        PaymentMethod = "pix",
                        // Gathering Pix Payment Information       
                        Pix = new CreatePixPaymentRequest
                        {
                            //ExpiresAt = DateTime.Now,
                            ExpiresIn = 360,
                            AdditionalInformation = new List<PixAdditionalInformation>()
                        }

                    };
                    ret.Pix.AdditionalInformation.Add(new PixAdditionalInformation()
                    {
                        Name = order.Customer.Name,
                        Value = order.Value.ToString()
                    });
                    break;
            }

            ret.Split = new List<CreateSplitRequest>
                        {
                            new CreateSplitRequest {
                                Amount = (100 - Int32.Parse(Amount)),
                                RecipientId = this.pagarMeConfig.RecipientIdSmartSales,
                                Type = pagarMeStoreConfig.Type,
                                Options = new CreateSplitOptionsRequest()
                                {
                                    ChargeRemainderFee = true,
                                    ChargeProcessingFee = true,
                                    Liable = true
                                }
                            },
                            new CreateSplitRequest
                            {
                                Amount = Int32.Parse(Amount),
                                RecipientId = pagarMeStoreConfig.RecipientIdLojista,
                                Type = pagarMeStoreConfig.Type,
                                Options = new CreateSplitOptionsRequest()
                                {
                                    ChargeRemainderFee = Boolean.Parse(pagarMeStoreConfig.ChargeRemainderFee),
                                    ChargeProcessingFee = Boolean.Parse(pagarMeStoreConfig.ChargeProcessingFee),
                                    Liable = Boolean.Parse(pagarMeStoreConfig.Liable)
                                }
                            }
                        };

            return ret;
        }

        public async Task<KeyValuePair<string, ServiceResponse<PaymentDto>>> CreateOrder(Order order, List<OrderItem> items, OrderShipping shipping, PaymentCardDto paymentCardDto, Customer customer, PaymentTypeEnum paymentTypeId)
        {
            this._logger.LogInformation("Create Order from GatewayServices");

            var payload_items = items.Select(x => new CreateOrderItemRequest
            {
                Code = x.ProductId.ToString().Replace("-", ""),
                Description = x.Description,
                Amount = (int)((x.GetUnitNetValue()) * 100),
                Quantity = x.Units
            }).ToList();

            if (customer.Phone.Length < 12) throw new Exception("Phone number is invalid.");

            var payload_customer = new CreateCustomerRequest
            {
                Type = "individual",
                Name = customer.Name,
                Document = customer.Cpf,
                Email = customer.Email,
                Phones = new CreatePhonesRequest
                {
                    HomePhone = new CreatePhoneRequest
                    {
                        AreaCode = customer.Phone.Substring(2, 2),
                        Number = customer.Phone[4..],
                        CountryCode = "55"
                    },
                    MobilePhone = new CreatePhoneRequest
                    {
                        AreaCode = customer.Phone.Substring(2, 2),
                        Number = customer.Phone[4..],
                        CountryCode = "55"
                    }
                }
            };

            var payload_address = new CreateAddressRequest
            {
                Line1 = shipping.Address.StreetName + " " + shipping.Address.StreetNumber,
                Line2 = shipping.Address.Complement + " " + shipping.Address.Reference,
                ZipCode = shipping.Address.ZipCode,
                City = shipping.Address.CityName,
                State = shipping.Address.StateName,
                Country = "BR"
            };

            var payload_location = new CreateLocationRequest
            {
                Latitude = "-22.970722",
                Longitude = "43.182365"
            };

            //// Gathering Shipping Information
            var payload_shipping = new CreateShippingRequest
            {
                Amount = (int)(order.ShippingAmmount * 100),
                Description = "Entrega",
                RecipientName = customer.Name,
                RecipientPhone = customer.Phone,
                Address = payload_address
            };

            // Gathering Antifraud Clearsale Information
            var payload_antifraud_clearsale = new CreateClearSaleRequest
            {
                CustomSla = 90
            };

            // Gathering Antifraud Information
            var payload_antifraud = new CreateAntifraudRequest
            {
                Type = "clearsale",
                Clearsale = payload_antifraud_clearsale,
            };

            // Gathering Device Information
            var payload_device = new CreateDeviceRequest
            {
                Platform = "ANDROID/OS"
            };

            // Gathering Billing Address Information
            var payload_billing_address = payload_address;
            CreateCardRequest payload_card = null;
            if (paymentCardDto != null)
            {
                // Gathering Card Information
                payload_card = new CreateCardRequest
                {
                    Number = paymentCardDto.Number,
                    HolderName = paymentCardDto.Holder,
                    ExpMonth = paymentCardDto.Month.ToInt(),
                    ExpYear = paymentCardDto.Year.ToInt(),
                    Cvv = paymentCardDto.SecurityCode,
                    BillingAddress = payload_billing_address
                };
            }

            // Gathering Credit Card Payment Information
            CreatePaymentRequest payload_payment = CreatePaymentPayload(order, payload_card, paymentTypeId, paymentCardDto);

            // Gathering Payments Information
            var payload_payments = new List<CreatePaymentRequest>
            {
                payload_payment
            };

            // Gathering Request Information
            var payload = new CreateOrderRequest
            {
                Shipping = payload_shipping,
                Items = payload_items,
                Customer = payload_customer,
                Ip = "52.168.67.32",
                Location = payload_location,
                Antifraud = payload_antifraud,
                SessionId = "322b821a",
                Device = payload_device,
                Payments = payload_payments
            };

            var responseData = new ServiceResponse<PaymentDto>
            {
                Message = "Falha no envio ao gateway",
                IsSuccess = false
            };

            try
            {
                this._logger.LogInformation("Creating payment message with PagarMe");

                var client = new PagarmeCoreApiClient(this.pagarMeConfig.BasicAuthUserName, this.pagarMeConfig.BasicAuthPassword);

                var PagarMeResponse = client.Orders.CreateOrder(payload);

                if (PagarMeResponse.Charges.Count == 0)
                    throw new Exception(PagarMeResponse.Charges.Count.ToString());
                var LastTransaction = PagarMeResponse.Charges[0].LastTransaction;
                var GatewayResponse = LastTransaction.GatewayResponse;
                var GatewayResponseErrors = GatewayResponse.Errors;
                order.OrderCode = PagarMeResponse.Id;

                if (GatewayResponseErrors == null || GatewayResponseErrors.Count == 0)
                {
                    var response = new PaymentDto
                    {
                        // Amount = PagarMeResponse.Amount,  
                        Currency = PagarMeResponse.Currency,
                        Installments = paymentCardDto.Installments,
                        GatewayProvider = "PagarMe (Mundipagg)",
                        Status = FixPaymentStatus(PagarMeResponse.Status),
                        PaymentTypeId = (int)paymentTypeId,
                        Transactions = PagarMeResponse.Charges.Select(transaction => new TransactionDto
                        {
                            Ammount = transaction.Amount,
                            Currency = transaction.Currency,
                            Installments = paymentCardDto.Installments,
                            IsSuccess = transaction.LastTransaction.Success,
                            Status = FixTransactionStatus(transaction.Status),
                            Nsu = PagarMeResponse.Code
                        }).ToList()
                    };

                    responseData = new ServiceResponse<PaymentDto>
                    {
                        Errors = GatewayResponseErrors != null ? GatewayResponseErrors.Select(error => error.Message).ToList() : null,
                        Message = "Envio ao gateway efetuado",
                        IsSuccess = !(GatewayResponseErrors != null ? GatewayResponseErrors.Any() : false) && (response?.Transactions?.Any(x => x.IsSuccess) ?? false),
                        ResponseData = response /*,
                        AuthenticationUrl = LastTransaction.ThreedAuthenticatioUrl,     
                        QRCode = LastTransaction.QRCode,
                        QRCodeUrl = LastTransaction.QRCodeUrl
                        */
                    };

                    if (response?.Transactions?.Any(x => x.Status == "REJECT") == true)
                    {
                        responseData.Errors.Add("Transação Rejeitada");
                    }
                }
                else throw new Exception("Lista de erros não é vazia: " + GatewayResponseErrors.ToString());

            }
            catch (Exception exception)
            {
                this._logger.LogCritical("Erro em requisição à integradora de cartões {@error}", exception);

                return new KeyValuePair<string, ServiceResponse<PaymentDto>>("", new ServiceResponse<PaymentDto>
                {
                    Message = "Erro em requisiçãoo à integradora de cartões [" + exception.Message + "]",
                    IsSuccess = false
                });
            }

            return new KeyValuePair<string, ServiceResponse<PaymentDto>>(order.OrderCode, responseData);
        }

        public async Task<ServiceResponse<PaymentDto>> GetOrderStatus(string orderCode)
        {
            var responseData = new ServiceResponse<PaymentDto>();

            try
            {

                var client = new PagarmeCoreApiClient(this.pagarMeConfig.BasicAuthUserName, this.pagarMeConfig.BasicAuthPassword);

                var PagarMeResponse = client.Orders.GetOrder(orderCode);

                if (PagarMeResponse.Charges.Count == 0)
                    throw new Exception(PagarMeResponse.Charges.Count.ToString());
                var LastTransaction = PagarMeResponse.Charges[0].LastTransaction;
                var GatewayResponse = LastTransaction?.GatewayResponse;
                var GatewayResponseErrors = GatewayResponse?.Errors;

                if (GatewayResponseErrors == null || GatewayResponseErrors?.Count == 0)
                {
                    var response = new PaymentDto
                    {
                        // Amount = PagarMeResponse.Amount,  
                        Currency = PagarMeResponse.Currency,
                        Installments = PagarMeResponse.Charges.Count,
                        GatewayProvider = "PagarMe (Mundipagg)",
                        Status = FixPaymentStatus(PagarMeResponse.Status),
                        Transactions = PagarMeResponse.Charges.Select(transaction => new TransactionDto
                        {
                            Ammount = transaction.Amount,
                            Currency = transaction.Currency,
                            Installments = PagarMeResponse.Charges.Count,
                            IsSuccess = transaction.LastTransaction == null ? true : transaction.LastTransaction.Success,
                            Status = FixTransactionStatus(transaction.Status),
                        }).ToList()
                    };

                    responseData = new ServiceResponse<PaymentDto>
                    {
                        Errors = GatewayResponseErrors != null ? GatewayResponseErrors.Select(error => error.Message).ToList() : null,
                        Message = "Envio ao gateway efetuado",
                        IsSuccess = !(GatewayResponseErrors != null ? GatewayResponseErrors.Any() : false) && (response?.Transactions?.Any(x => x.IsSuccess) ?? false),
                        ResponseData = response
                    };

                    if (response?.Transactions?.Any(x => x.Status == "REJECT") == true)
                    {
                        responseData.Errors.Add("Transação Rejeitada");
                    }
                }
                else throw new Exception("Lista de erros não é vazia: " + GatewayResponseErrors.ToString());

            }
            catch (Exception exception)
            {
                this._logger.LogCritical("Erro em requisição à integradora de cartões {@error}", exception);

                return new ServiceResponse<PaymentDto>
                {
                    Message = "Erro em requisiçãoo à integradora de cartões",
                    IsSuccess = false
                };
            }

            return responseData;
        }
    }
}