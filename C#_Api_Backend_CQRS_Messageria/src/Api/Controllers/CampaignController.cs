using CoreService.Infrastructure.Services;
using Infra.ExternalServices.Reshop.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Api.App_Infra;

namespace Api.Controllers
{

    [Route("api/v1/"), ApiController, ServiceFilter(typeof(SmartSalesAuthorizeAttribute))]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignIntegrationService _campaignService;

        public CampaignController(ICampaignIntegrationService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpPost, Route("ConsultaCampanha")]
        public async Task<ReshopConfirmsCampaignResponse> PostConsultaCampanha([FromBody] ReshopQueryCampaign content)
        {
            var response = await _campaignService.PostQueryCampaign(content);
            return response;
        }

        [HttpPost, Route("ProcessaCampanha")]
        public async Task<ReshopProcessCampaignResponse> PostProcessaCampanha([FromBody] ReshopProcessCampaign content)
        {
            var response = await _campaignService.PostProcessCampaign(content);
            return response;
        }

        [HttpGet, Route("ConfimaCampanha")]
        public async Task<ReshopConfirmsCampaign> GetConfirmaCampanha([FromQuery] string codigoLoja, [FromQuery] String nsu, [FromQuery] String saleNumber, [FromQuery] bool confirma)
        {
            var response = await _campaignService.GetConfirmsCampaigns(codigoLoja, nsu, saleNumber, confirma);
            return response;
        }
    }
}