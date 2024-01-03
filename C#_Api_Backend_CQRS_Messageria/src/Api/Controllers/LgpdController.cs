using System;
using Api.App_Infra;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Infra.ExternalServices.Reshop.Dtos;
using CoreService.Infrastructure.Services;

namespace Api.Controllers
{
    [ApiController, Route("api/v1/[controller]"), ServiceFilter(typeof(SmartSalesAuthorizeAttribute))]
    public class LgpdController : ControllerBase
    {
        public ILgpdIntegrationService LgpdIntegrationService { get; }

        public LgpdController(ILgpdIntegrationService lgpdIntegrationService)
        {
            LgpdIntegrationService = lgpdIntegrationService;
        }

        /// <summary>
        /// Retornar as finalidades LGPD liberadas para o cliente
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        [HttpGet, Route("GetPermission/{document}")]
        public async Task<CustomerPermission> GetPermission([FromRoute] String document)
        {
            var response = await LgpdIntegrationService.GetPermission(document);
            return response;
        }
    }
}
