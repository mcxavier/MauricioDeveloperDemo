using System;
using System.Net.Http;
using Newtonsoft.Json;
using Core.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using Infra.ExternalServices.Authentication;
using CoreService.Infrastructure.Services;
using Infra.ExternalServices.Reshop.Dtos;

namespace Infra.ExternalServices.Reshop
{
    public class ReshopIntegrationService : IReshopIntegrationService
    {
        private readonly HttpClient _httpClient;
        private readonly SmartSalesIdentity _identity;
        private readonly IStoreRepository _storeRepository;

        public ReshopIntegrationService(SmartSalesIdentity identity, IStoreRepository storeRepository, HttpClient httpClient)
        {
            this._storeRepository = storeRepository;
            this._httpClient = httpClient;
            this._identity = identity;
        }

        public async Task<ReshopToken> GetToken()
        {
            var settings = await this._storeRepository.GetStoreCampaignSettingsAsync(_identity.CurrentStoreId ?? Guid.Empty);
            var username = settings?.UserName;
            var password = settings?.Password;

            var values = new Dictionary<string, string>
            {
                { nameof(username), username },
                { nameof(password), password },
                { "grant_type", "password" }
            };

            this._httpClient.BaseAddress = new Uri(settings?.ServiceUrl ?? "");
            var responseSend = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "token") { Content = new FormUrlEncodedContent(values) });
            var responseString = await responseSend.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ReshopToken>(responseString);

            response.BaseAddress = this._httpClient.BaseAddress;

            return response;
        }
    }
}