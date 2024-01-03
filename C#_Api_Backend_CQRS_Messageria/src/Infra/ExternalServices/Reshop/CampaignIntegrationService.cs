using System;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Infra.ExternalServices.Reshop.Dtos;
using CoreService.Infrastructure.Services;
using Infra.ExternalServices.Authentication;

namespace Infra.ExternalServices.Reshop
{
    public class CampaignService : ICampaignIntegrationService
    {
        private readonly IReshopIntegrationService reshopIntegrationService;
        private readonly SmartSalesIdentity identity;
        private readonly HttpClient httpClient;

        public CampaignService(IReshopIntegrationService reshopIntegrationService, SmartSalesIdentity identity, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.reshopIntegrationService = reshopIntegrationService;
            this.identity = identity;
        }

        public async Task<ReshopConfirmsCampaignResponse> PostQueryCampaign(ReshopQueryCampaign content)
        {
            content.CodigoLoja = identity?.CurrentStoreCode;
            content.DataHora = DateTime.Now;

            var token = await reshopIntegrationService.GetToken();
            httpClient.BaseAddress = token.BaseAddress;
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.access_token}");

            var requestBody = JsonConvert.SerializeObject(content);
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/Fidelidade/ConsultaCampanha")
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            });

            try
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var queryResponseObj = JsonConvert.DeserializeObject<ReshopConfirmsCampaignResponse>(responseString);
                return queryResponseObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ReshopProcessCampaignResponse> PostProcessCampaign(ReshopProcessCampaign content)
        {
            var token = await reshopIntegrationService.GetToken();
            httpClient.BaseAddress = token.BaseAddress;
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.access_token}");

            var requestBody = JsonConvert.SerializeObject(content);
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/Fidelidade/ProcessaOperacao")
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            });

            try
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var operationResponseObj = JsonConvert.DeserializeObject<ReshopProcessCampaignResponse>(responseString);
                return operationResponseObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ReshopConfirmsCampaign> GetConfirmsCampaigns(string codigoLoja, string nsu, string saleNumber, bool confirma)
        {
            var token = await reshopIntegrationService.GetToken();
            httpClient.BaseAddress = token.BaseAddress;
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.access_token}");

            var response = await httpClient.GetFromJsonAsync<ReshopConfirmsCampaign>($"api/Fidelidade/ConfirmaOperacao?codigoLoja={codigoLoja}&nsu={nsu}&saleNumber={saleNumber}&confirma={confirma}");

            return response;
        }
    }
}