using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Repositories;
using LinxIO.Dtos;
using System.Net.Http.Headers;
using System.Text;
using Infra.QueryCommands.Commands.Topics;
using Core.SharedKernel;
using MediatR;
using Infra.Domains.Topics;
using System.Linq;
using Infra.ExternalServices.Authentication;
using Core.Models.Core.Ordering;
using Infra.ExternalServices.Catalog;
using CoreService.IntegrationsViewModels;
using Utils;

namespace LinxIO.Services
{
    public class LinxIOService : System.IDisposable
    {
        private readonly ILogger logger;
        private readonly SmartSalesIdentity identity;
        private readonly IProductIntegrationService _productsIntegrationService;
        private readonly HttpClient httpClient;
        private readonly ICompanyRepository companyRepository;
        private Dictionary<Guid, LinxIOToken> tokens;
        private readonly IMediator mediator;

        public LinxIOService(ILogger logger, SmartSalesIdentity identity, ICompanyRepository companyRepository, IProductIntegrationService productsIntegration, IMediator mediator)
        {
            this.companyRepository = companyRepository;
            this._productsIntegrationService = productsIntegration;
            this.httpClient = new HttpClient();
            this.logger = logger;
            this.identity = identity;
            this.mediator = mediator;
            this.tokens = new Dictionary<Guid, LinxIOToken>();
            logger.LogInformation("LinxIO Service Started.");
        }

        private async Task<string> GetToken(Guid companyId)
        {
            if (!this.tokens.ContainsKey(companyId))
            {
                var username = identity.LinxIOUsername;
                var password = identity.LinxIOPassword;

                var values = new Dictionary<string, string>
                {
                    { nameof(username), username },
                    { nameof(password), password }
                };

                var request = new HttpRequestMessage(HttpMethod.Post, LinxIOEnvironment.TokenEndpoint) { Content = new FormUrlEncodedContent(values) };
                this.httpClient.BaseAddress = new Uri(LinxIOEnvironment.BaseAddress);

                for (int i = 0; i < 3; i++)
                {
                    var response = await httpClient.SendAsync(request);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var resultToken = JsonConvert.DeserializeObject<LinxIOToken>(responseString);
                    if (!string.IsNullOrEmpty(resultToken.Token))
                    {
                        this.tokens.Add(companyId, resultToken);
                        break;
                    }
                }
            }

            return this.tokens[companyId]?.Token;
        }

        public async Task<LinxIOTopicsResponse> GetTopics(Guid companyId)
        {
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken(companyId));
            this.httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };

            var response = await this.httpClient.GetAsync(LinxIOEnvironment.GetListagemEndpoint(identity.LinxIOApplicationId));
            var responseString = await response.Content.ReadAsStringAsync();
            var topicos = JsonConvert.DeserializeObject<LinxIOTopicsResponse>(responseString);

            if (topicos.allowedTopics.Count > topicos.queues.Count)
            {
                LinxIOTopics topic = new LinxIOTopics();

                foreach (var allowTopic in topicos.allowedTopics)
                {
                    topic.producerApplicationId = allowTopic.producerApplicationId;
                    topic.topicId = allowTopic.topicId;

                    if (!topicos.queues.Any(x => x.producerApplicationId == topic.producerApplicationId && x.topicId == topic.topicId))
                    {
                        var resp = this.SubscribeTopic(allowTopic.producerApplicationId, allowTopic.topicId, identity.LinxIOApplicationId);
                    }
                }

                response = await this.httpClient.GetAsync(LinxIOEnvironment.GetListagemEndpoint(identity.LinxIOApplicationId));
                responseString = await response.Content.ReadAsStringAsync();
                topicos = JsonConvert.DeserializeObject<LinxIOTopicsResponse>(responseString);
            }

            return topicos;
        }

        public async Task<Response> ConsumeQueue(LinxIOTopics queue, Guid companyId)
        {
            Response messageResponse = new Response();

            try
            {
                #region Set http
                string jsonVisibilityTimeout = JsonConvert.SerializeObject(new { visibilityTimeout = 1800 });
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken(companyId));
                this.httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
                #endregion

                for (int i = 0; i < queue.approximateNumberOfMessages; i++)
                {
                    var response = await this.httpClient.PostAsync(LinxIOEnvironment.GetQueueConsumeEndpoint(queue.queueId), new StringContent(jsonVisibilityTimeout, Encoding.UTF8, "application/json"));
                    var responseString = await response.Content.ReadAsStringAsync();

                    try
                    {
                        switch (queue.topicId)
                        {
                            case "price":
                                {
                                    var mensagens = JsonConvert.DeserializeObject<LinxIOListConsumeMessages<PublishApplicationPrice>>(responseString);
                                    foreach (var mensagem in mensagens.messages)
                                    {
                                        var price = (PublishApplicationPrice)mensagem.body;
                                        messageResponse = await mediator.Send(price);

                                        await ConfirmMsg(queue.queueId, price.SkuId.ToString(), mensagem.receiptId, mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message);
                                        await LogaMensagem(queue.topicId, price.SkuId.ToString(), mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message, responseString);
                                    }
                                }
                                break;
                            case "product":
                                {
                                    var mensagens = JsonConvert.DeserializeObject<LinxIOListConsumeMessages<PublishApplicationProduct>>(responseString);
                                    foreach (var mensagem in mensagens.messages)
                                    {
                                        var product = (PublishApplicationProduct)mensagem.body;
                                        messageResponse = await mediator.Send(product);

                                        await ConfirmMsg(queue.queueId, product.ProductId.ToString(), mensagem.receiptId, mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message);
                                        await LogaMensagem(queue.topicId, product.ProductId.ToString(), mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message, responseString);
                                    }
                                }
                                break;
                            case "stock":
                                {
                                    var mensagens = JsonConvert.DeserializeObject<LinxIOListConsumeMessages<PublishApplicationStock>>(responseString);
                                    foreach (var mensagem in mensagens.messages)
                                    {
                                        var stock = (PublishApplicationStock)mensagem.body;
                                        messageResponse = await mediator.Send(stock);

                                        await ConfirmMsg(queue.queueId, stock.SkuId.ToString(), mensagem.receiptId, mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message);
                                        await LogaMensagem(queue.topicId, stock.SkuId.ToString(), mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message, responseString);
                                    }
                                }
                                break;
                            case "sku":
                                {
                                    var mensagens = JsonConvert.DeserializeObject<LinxIOListConsumeMessages<PublishApplicationSku>>(responseString);
                                    foreach (var mensagem in mensagens.messages)
                                    {
                                        var sku = (PublishApplicationSku)mensagem.body;
                                        messageResponse = await mediator.Send(sku);

                                        await ConfirmMsg(queue.queueId, sku.SkuId.ToString(), mensagem.receiptId, mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message);
                                        await LogaMensagem(queue.topicId, sku.SkuId.ToString(), mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message, responseString);
                                    }
                                }
                                break;
                            case "location":
                                {
                                    var mensagens = JsonConvert.DeserializeObject<LinxIOListConsumeMessages<PublishApplicationLocation>>(responseString);
                                    foreach (var mensagem in mensagens.messages)
                                    {
                                        var store = (PublishApplicationLocation)mensagem.body;
                                        messageResponse = await mediator.Send(store);

                                        await ConfirmMsg(queue.queueId, store.LocationId.ToString(), mensagem.receiptId, mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message);
                                        await LogaMensagem(queue.topicId, store.LocationId.ToString(), mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message, responseString);
                                    }
                                }
                                break;
                            case "customer":
                                {
                                    var mensagens = JsonConvert.DeserializeObject<LinxIOListConsumeMessages<PublishApplicationCustomer>>(responseString);
                                    foreach (var mensagem in mensagens.messages)
                                    {
                                        PublishApplicationCustomer customer = (PublishApplicationCustomer)mensagem.body;
                                        messageResponse = await mediator.Send(customer);

                                        await ConfirmMsg(queue.queueId, customer.customerId.ToString(), mensagem.receiptId, mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message);
                                        await LogaMensagem(queue.topicId, customer.customerId.ToString(), mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message, responseString);
                                    }
                                }
                                break;
                            case "carrier-order-notify":
                                {
                                    var mensagens = JsonConvert.DeserializeObject<LinxIOListConsumeMessages<PublishApplicationCarrierOrderNotify>>(responseString);
                                    foreach (var mensagem in mensagens.messages)
                                    {
                                        var carrierOrderNotify = (PublishApplicationCarrierOrderNotify)mensagem.body;
                                        messageResponse = await mediator.Send(carrierOrderNotify);

                                        await ConfirmMsg(queue.queueId, carrierOrderNotify.OrderId.ToString(), mensagem.receiptId, mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message);
                                        await LogaMensagem(queue.topicId, carrierOrderNotify.OrderId.ToString(), mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message, responseString);
                                    }
                                }
                                break;
                            case "order-exception":
                                {
                                    var mensagens = JsonConvert.DeserializeObject<LinxIOListConsumeMessages<PublishApplicationOrderException>>(responseString);
                                    foreach (var mensagem in mensagens.messages)
                                    {
                                        var orderException = (PublishApplicationOrderException)mensagem.body;
                                        messageResponse = await mediator.Send(orderException);

                                        await ConfirmMsg(queue.queueId, orderException.orderId.ToString(), mensagem.receiptId, mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message);
                                        await LogaMensagem(queue.topicId, orderException.orderId.ToString(), mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message, responseString);
                                    }
                                }
                                break;
                            case "fulfillment-status":
                                {
                                    var mensagens = JsonConvert.DeserializeObject<LinxIOListConsumeMessages<PublishApplicationFulfillmentStatus>>(responseString);
                                    foreach (var mensagem in mensagens.messages)
                                    {
                                        var fulfillmentStatus = (PublishApplicationFulfillmentStatus)mensagem.body;
                                        messageResponse = await mediator.Send(fulfillmentStatus);

                                        await ConfirmMsg(queue.queueId, fulfillmentStatus.orderId.ToString(), mensagem.receiptId, mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message);
                                        await LogaMensagem(queue.topicId, fulfillmentStatus.orderId.ToString(), mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message, responseString);
                                    }
                                }
                                break;
                            case "order-payment-status":
                                {
                                    var mensagens = JsonConvert.DeserializeObject<LinxIOListConsumeMessages<PublishApplicationOrderPaymentStatus>>(responseString);
                                    foreach (var mensagem in mensagens.messages)
                                    {
                                        var orderPaymentStatus = (PublishApplicationOrderPaymentStatus)mensagem.body;
                                        messageResponse = await mediator.Send(orderPaymentStatus);

                                        await ConfirmMsg(queue.queueId, orderPaymentStatus.OrderId.ToString(), mensagem.receiptId, mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message);
                                        await LogaMensagem(queue.topicId, orderPaymentStatus.OrderId.ToString(), mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message, responseString);
                                    }
                                }
                                break;
                            case "order-treatment":
                                {
                                    var mensagens = JsonConvert.DeserializeObject<LinxIOListConsumeMessages<PublishApplicationOrderTreatment>>(responseString);
                                    foreach (var mensagem in mensagens.messages)
                                    {
                                        var orderTreatment = (PublishApplicationOrderTreatment)mensagem.body;
                                        messageResponse = await mediator.Send(orderTreatment);

                                        await ConfirmMsg(queue.queueId, orderTreatment.OrderId.ToString(), mensagem.receiptId, mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message);
                                        await LogaMensagem(queue.topicId, orderTreatment.OrderId.ToString(), mensagem.messageId, messageResponse.IsError ? AckType.ERROR : AckType.SUCCESS, messageResponse.Message, responseString);
                                    }
                                }
                                break;
                            case "feedback":
                                {
                                    var mensagens = JsonConvert.DeserializeObject<LinxIOListConsumeMessages<PublishApplicationFeedBack>>(responseString);
                                    foreach (var mensagem in mensagens.messages)
                                    {
                                        var feedBack = (PublishApplicationFeedBack)mensagem.body;
                                        messageResponse = (Response)await mediator.Send(feedBack);

                                        await ConfirmFeedBack(queue.queueId, mensagem.receiptId, mensagem.messageId);
                                        await LogaMensagem(queue.topicId, feedBack.entityId, mensagem.messageId, (feedBack.Type.CompareTo("ERROR") == 0) ? AckType.ERROR : AckType.SUCCESS, feedBack.message, responseString);
                                    }
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        var result = JsonConvert.DeserializeObject<LinxIOListConsumeMessages<object>>(responseString);
                        await LogaMensagem(queue.topicId, result.messages?.First()?.messageId ?? "", result.messages?.First()?.messageId ?? "", AckType.ERROR, ex.Message, responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Erro ConsumeQueue", ex);
            }

            return messageResponse;
        }

        public async Task<Response> SendQueue(Guid companyId)
        {
            Response messageResponse = new Response();

            try
            {
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken(companyId));
                this.httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };

                #region Order
                var sendOrder = await mediator.Send(new PublishApplicationOrder());
                if (!sendOrder.IsError)
                {
                    foreach (var pubOrder in (List<PublishApplicationOrder>)sendOrder.Payload)
                    {
                        var pubOrderString = JsonConvert.SerializeObject(pubOrder);
                        var httpContent = new StringContent(pubOrderString, Encoding.UTF8, "application/json");
                        var httpResponseMessage = await this.httpClient.PostAsync(LinxIOEnvironment.GetPublishEndpoint(identity.LinxIOApplicationId, "order"), httpContent);

                        var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<LinxIOPublishResponse>(responseString);

                        await LogaMensagem("order", pubOrder.orderId, response.messageId, response.IsError ? AckType.ERROR : AckType.SUCCESS, response.IsError ? responseString : "Pedido enviado com sucesso", pubOrderString);

                        await mediator.Send(new PublishApplicationOrderStatus()
                        {
                            OrderId = pubOrder.orderId,
                            Status = response.IsError ? OrderStatus.IntegrationFailed : OrderStatus.Integrated,
                            UpdatedAt = DateTime.Now
                        });
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                messageResponse.IsError = true;
                messageResponse.Message = ex.Message;
            }

            return messageResponse;
        }

        public async Task<Response> GetVtexImages(Guid companyId)
        {
            Response messageResponse = new Response();

            try
            {
                var config = companyRepository.GetCompanySettingsAsync(companyId, Core.Models.Identity.Companies.CompanySettingsType.LinxIOConfig)?.Result;
                var values = string.IsNullOrEmpty(config?.Value) ? new LinxIOVTexClientConfig() : JsonConvert.DeserializeObject<LinxIOVTexClientConfig>(config.Value, Utils.JsonSettings.Settings);
                if (values.SyncImg)
                {
                    var variantIdList = await mediator.Send(new PublishApplicationVtexVariantList());
                    if (!variantIdList.IsError)
                    {
                        foreach (PublishApplicationVtexVariantList variantReg in (List<PublishApplicationVtexVariantList>)variantIdList.Payload)
                        {
                            var variantImagesList = await this._productsIntegrationService.GetImagetVariationsById(variantReg.variationId, variantReg.skuId);

                            foreach (VtexImageVariation imgVar in variantImagesList)
                            {
                                await mediator.Send(new PublishApplicationVtexVariantSave()
                                {
                                    name = imgVar.name,
                                    urlImage = imgVar.urlImage,
                                    isPrincipal = imgVar.isPrincipal,
                                    productVariationId = variantReg.variationId
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                messageResponse.IsError = true;
                messageResponse.Message = ex.Message;
            }

            return messageResponse;
        }

        public async Task<Response> LogaMensagem(string topicId, string entityId, string referenceMessageId, AckType type, string msg, string msgJson)
        {
            PublishApplicationLogGeral logGeral = new PublishApplicationLogGeral
            {
                TopicId = topicId,
                EntityId = entityId,
                ReferenceMessageId = referenceMessageId,
                Type = type.ToString(),
                Message = msg,
                MessageJson = msgJson,
                DataHora = DateTimeBrazil.Now
            };

            var messageResponse = (Response)await mediator.Send(logGeral);

            return messageResponse;
        }

        public async Task<Response> ConfirmMsg(string queueId, string entityId, string receiptId, string referenceMessageId, AckType type, string msg)
        {
            Response messageResponse = new Response();

            try
            {
                PublishApplicationAck ack = new PublishApplicationAck
                {
                    entityId = entityId,
                    receiptId = receiptId,
                    referenceMessageId = referenceMessageId,
                    type = type.ToString(),
                };

                ack.feedbackContent = new FeedbackContent()
                {
                    additionalInfo = new AdditionalInfo() { description = type.ToString() },
                    message = msg
                };

                if (type == AckType.ERROR)
                {
                    ack.type = AckType.DISCARD.ToString();
                }

                var temp1 = new StringContent(JsonConvert.SerializeObject(ack), Encoding.UTF8, "application/json");
                var temp3 = await temp1.ReadAsStringAsync();
                var temp2 = LinxIOEnvironment.GetSendAckEndpoint(queueId);

                var httpResponseMessage = await this.httpClient.PostAsync(
                    LinxIOEnvironment.GetSendAckEndpoint(queueId),
                    new StringContent(JsonConvert.SerializeObject(ack), Encoding.UTF8, "application/json"));

                string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                messageResponse.Message = responseString;
            }
            catch (Exception e)
            {
                messageResponse.IsError = true;
                messageResponse.Message = e.Message;
            }

            return messageResponse;
        }

        public async Task<Response> ConfirmFeedBack(string queueId, string receiptId, string referenceMessageId)
        {
            Response messageResponse = new Response();

            try
            {
                PublishApplicationFeedBackAck ack = new PublishApplicationFeedBackAck
                {
                    receiptId = receiptId,
                    referenceMessageId = referenceMessageId
                };

                var httpResponseMessage = await this.httpClient.PostAsync(
                    LinxIOEnvironment.GetSendAckEndpoint(queueId),
                    new StringContent(JsonConvert.SerializeObject(ack), Encoding.UTF8, "application/json"));

                string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                messageResponse.Message = responseString;
            }
            catch (Exception e)
            {
                messageResponse.IsError = true;
                messageResponse.Message = e.Message;
            }

            return messageResponse;
        }

        public async Task<Response> SubscribeTopic(string producerApplicationId, string topicId, string applicationId)
        {
            Response messageResponse = new Response();

            try
            {
                PublishApplicationSubscribeTopic subscribeTopic = new PublishApplicationSubscribeTopic()
                {
                    producerApplicationId = producerApplicationId
                };

                var httpResponseMessage = await this.httpClient.PostAsync(
                    LinxIOEnvironment.GetSubrscribeTopic(topicId, applicationId),
                    new StringContent(JsonConvert.SerializeObject(subscribeTopic), Encoding.UTF8, "application/json"));

                string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                messageResponse.Message = responseString;
            }
            catch (Exception e)
            {
                messageResponse.IsError = true;
                messageResponse.Message = e.Message;
            }

            return messageResponse;
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}