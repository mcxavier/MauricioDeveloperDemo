using CoreService.Infrastructure.Services;
using Infra.ExternalServices.Reshop.Dtos;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Infra.ExternalServices.Reshop
{
    public class LgpdIntegrationService : ILgpdIntegrationService
    {
        private readonly HttpClient httpClient;
        private readonly IReshopIntegrationService reshopIntegrationService;

        public LgpdIntegrationService(HttpClient httpClient, IReshopIntegrationService reshopIntegrationService)
        {
            this.httpClient = httpClient;
            this.reshopIntegrationService = reshopIntegrationService;
        }

        public async Task<CustomerPermission> GetPermission(string document)
        {
            var token = await reshopIntegrationService.GetToken();
            httpClient.BaseAddress = token.BaseAddress;
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.access_token}");

            var response = await httpClient.GetFromJsonAsync<CustomerPermission>($"api/customer/GetCustomer?document={document}&CustomerFinality=true");

            return response;
        }
    }
}