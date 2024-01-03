using System;
using System.Threading.Tasks;
using Infra.ExternalServices.Reshop.Dtos;

namespace CoreService.Infrastructure.Services
{
    public interface ICampaignIntegrationService
    {
        Task<ReshopConfirmsCampaignResponse> PostQueryCampaign(ReshopQueryCampaign contexto);
        Task<ReshopProcessCampaignResponse> PostProcessCampaign(ReshopProcessCampaign contexto);
        Task<ReshopConfirmsCampaign> GetConfirmsCampaigns(String codigoLoja, String nsu, String saleNumber, bool confirma);
    }
}