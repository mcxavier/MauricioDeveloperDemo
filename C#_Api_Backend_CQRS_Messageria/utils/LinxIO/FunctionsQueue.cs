using System;
using MediatR;
using LinxIO.Dtos;
using LinxIO.Services;
using Core.Repositories;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Infra.ExternalServices.Catalog;
using Infra.ExternalServices.Authentication;
using System.Linq;
using Newtonsoft.Json;
using Utils.Extensions;
using LinxIO.Interfaces;
using Utils;

namespace LinxIO
{
    public class FunctionsQueue
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IProductIntegrationService _productsIntegrationService;
        private readonly IQueueService _queueService;
        private readonly SmartSalesIdentity _identity;
        private readonly IMediator _mediator;

        public FunctionsQueue(ICompanyRepository companyRepository, IMediator mediator, IProductIntegrationService productsIntegration, SmartSalesIdentity identity, IQueueService queueService)
        {
            this._companyRepository = companyRepository;
            this._productsIntegrationService = productsIntegration;
            this._mediator = mediator;
            this._identity = identity;
            this._queueService = queueService;
        }

        public void ProcessQueuePoolingMessage([QueueTrigger("linxio-queue")] string message, ILogger logger)
        {
            var companyId = new Guid(message);

            if (!SetIdentity(companyId))
                logger.LogError(message, "Falha ao carregar parâmetros");

            using (var service = new LinxIOService(logger, _identity, _companyRepository, _productsIntegrationService, _mediator))
            {
                try
                {
                    #region Recebe
                    var topics = service.GetTopics(companyId).Result;

                    foreach (var queue in topics.queues.Where(x => x.approximateNumberOfMessages > 0))
                    {
                        var msg = new LinxIOConsumeRequest()
                        {
                            CompanyId = companyId,
                            Topics = queue
                        };

                        _queueService.SendMessage("linxio-consume", msg.AsJson());
                    }
                    #endregion

                    #region Envia
                    var result = service.SendQueue(companyId).Result;
                    #endregion

                    #region Busca Imagens VTEX
                    var resultVtex = service.GetVtexImages(companyId).Result;
                    #endregion
                }
                catch (Exception ex)
                {
                    logger.LogError(message, ex);
                }
            }

            logger.LogInformation(message);
        }

        public void CosumeQueuesPoolingMessage([QueueTrigger("linxio-consume")] string message, ILogger logger)
        {
            var dto = JsonConvert.DeserializeObject<LinxIOConsumeRequest>(message);

            if (!SetIdentity(dto.CompanyId))
                logger.LogInformation(message);

            try
            {
                using (var service = new LinxIOService(logger, _identity, _companyRepository, _productsIntegrationService, _mediator))
                {
                    var consumeQueues = service.ConsumeQueue(dto.Topics, dto.CompanyId).Result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(message, ex);
            }
            finally
            {
                logger.LogInformation(message);
            }
        }

        private bool SetIdentity(Guid companyId)
        {
            var configured = true;
            var company = _companyRepository.GetCompanyByIdAsync(companyId).Result;

            _identity.CurrentCompany = companyId;
            _identity.CurrentCompanyName = company?.FullName ?? "";
            _identity.Name = "user.app";
            _identity.IsCustomer = false;
            _identity.AuthenticationType = "Operator";
            _identity.IsAuthenticated = true;

            try
            {
                var config = _companyRepository.GetCompanySettingsAsync(companyId, Core.Models.Identity.Companies.CompanySettingsType.LinxIOConfig).Result;
                var values = JsonConvert.DeserializeObject<LinxIOClientConfig>(config.Value, JsonSettings.Settings);
                _identity.LinxIOApplicationId = values.ApplicationId;
                _identity.LinxIOUsername = values.Username;
                _identity.LinxIOPassword = values.Password;
            }
            catch (Exception)
            {
                configured = false;
            }

            return configured;
        }
    }
}
