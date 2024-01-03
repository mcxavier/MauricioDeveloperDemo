using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using CoreService.Infrastructure.Services;
using CoreService.IntegrationsViewModels;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using System.Collections.Generic;
using Utils;
using Infra.ExternalServices.Authentication;
using Core.Repositories;

namespace Infra.ExternalServices.Catalog
{

    public class ProductService : IProductIntegrationService
    {

        private readonly ILogger<ProductService> _logger;
        private readonly HttpClient _httpClient;
        private readonly SmartSalesIdentity _identity;
        private readonly ICompanyRepository _companyRepository;
        private LinxIOVTexClientConfig vtexConfig;

        public ProductService(SmartSalesIdentity identity, ICompanyRepository companyRepository, ILogger<ProductService> logger, HttpClient httpClient)
        {
            this._httpClient = httpClient;
            this._logger = logger;
            this._identity = identity;
            this._companyRepository = companyRepository;
        }

        public class LinxIOVTexClientConfig
        {
            public string BaseUrl { get; set; }
            public string SecondUrl { get; set; }
            public string XvtexAppKey { get; set; }
            public string XvtexAppToken { get; set; }
        }

        public async Task<VtexProduct> GetById(int id)
        {
            var config = _companyRepository.GetCompanySettingsAsync((this._identity.CurrentCompany ?? Guid.Empty), Core.Models.Identity.Companies.CompanySettingsType.VTexClientConfig).Result;
            this.vtexConfig = JsonConvert.DeserializeObject<LinxIOVTexClientConfig>(config.Value, JsonSettings.Settings);

            this._httpClient.BaseAddress = new Uri(vtexConfig.BaseUrl);
            this._httpClient.DefaultRequestHeaders.Add("x-vtex-api-appkey", this.vtexConfig.XvtexAppKey);
            this._httpClient.DefaultRequestHeaders.Add("x-vtex-api-apptoken", this.vtexConfig.XvtexAppToken);

            this._logger.LogInformation("Requesting vtex-product {0}", id);
            var response = await this._httpClient.GetFromJsonAsync<VtexProduct>($"api/catalog_system/pvt/products/ProductGet/{id}");
            this._logger.LogTrace("response vtex-product {id} {@response}", id, response);

            return response;
        }

        public async Task<VtexProductVariation> GetProductVariationsById(int id)
        {
            var config = _companyRepository.GetCompanySettingsAsync((this._identity.CurrentCompany ?? Guid.Empty), Core.Models.Identity.Companies.CompanySettingsType.VTexClientConfig).Result;
            this.vtexConfig = JsonConvert.DeserializeObject<LinxIOVTexClientConfig>(config.Value, JsonSettings.Settings);

            this._httpClient.BaseAddress = new Uri(vtexConfig.BaseUrl);
            this._httpClient.DefaultRequestHeaders.Add("x-vtex-api-appkey", this.vtexConfig.XvtexAppKey);
            this._httpClient.DefaultRequestHeaders.Add("x-vtex-api-apptoken", this.vtexConfig.XvtexAppToken);

            this._logger.LogInformation("Requesting vtex-variation {0}", id);
            var response = await this._httpClient.GetFromJsonAsync<VtexProductVariation>($"api/catalog_system/pvt/sku/stockkeepingunitbyid/{id}");
            this._logger.LogTrace("response vtex-variation {id} {@response}", id, response);

            return response;
        }

        public async Task<List<VtexImageVariation>> GetImagetVariationsById(string id, string skuId)
        {
            var config = _companyRepository.GetCompanySettingsAsync((this._identity.CurrentCompany ?? Guid.Empty), Core.Models.Identity.Companies.CompanySettingsType.VTexClientConfig).Result;
            this.vtexConfig = JsonConvert.DeserializeObject<LinxIOVTexClientConfig>(config.Value, JsonSettings.Settings);

            this._httpClient.BaseAddress = new Uri(vtexConfig.BaseUrl);
            this._httpClient.DefaultRequestHeaders.Add("x-vtex-api-appkey", this.vtexConfig.XvtexAppKey);
            this._httpClient.DefaultRequestHeaders.Add("x-vtex-api-apptoken", this.vtexConfig.XvtexAppToken);

            List<VtexImageVariation> lista = new List<VtexImageVariation>();

            this._logger.LogInformation("Requesting vtex-variation {0}", id);

            try
            {
                var response = await this._httpClient.GetFromJsonAsync<VtexProductVariation>($"api/catalog_system/pvt/sku/stockkeepingunitbyid/" + skuId);

                if (response.ImageUrl.Length > 0)
                {
                    lista.Add(new VtexImageVariation()
                    {
                        name = response.NameComplete,
                        urlImage = response.ImageUrl,
                        isPrincipal = true,
                        productVariationId = id
                    });
                }

                if (response.Images != null)
                {
                    foreach (VtexProductVariationImage img in response.Images)
                    {
                        lista.Add(new VtexImageVariation()
                        {
                            name = response.NameComplete,
                            urlImage = img.ImageUrl,
                            isPrincipal = false,
                            productVariationId = id
                        });
                    }
                }


                this._logger.LogTrace("response vtex-variation {id} {@response}", id, response);
            }
            catch (Exception ex)
            {

            }



            return lista;
        }


        public async Task<VtexProductsIds> GetProductIds(int currentPage = 1, int pageSize = 50)
        {
            var config = _companyRepository.GetCompanySettingsAsync((this._identity.CurrentCompany ?? Guid.Empty), Core.Models.Identity.Companies.CompanySettingsType.VTexClientConfig).Result;
            this.vtexConfig = JsonConvert.DeserializeObject<LinxIOVTexClientConfig>(config.Value, JsonSettings.Settings);

            this._httpClient.BaseAddress = new Uri(vtexConfig.BaseUrl);
            this._httpClient.DefaultRequestHeaders.Add("x-vtex-api-appkey", this.vtexConfig.XvtexAppKey);
            this._httpClient.DefaultRequestHeaders.Add("x-vtex-api-apptoken", this.vtexConfig.XvtexAppToken);

            this._logger.LogInformation("Requesting vtex-ids {page}", currentPage);
            var response = await _httpClient.GetFromJsonAsync<VtexProductsIds>($"api/catalog_system/pvt/products/GetProductAndSkuIds?_from={currentPage}&_to={pageSize}");
            this._logger.LogTrace("Response vtex-ids {currentPage}", currentPage, response);

            return response;
        }

        public async Task<VtexBasePrices> GetProductVariationPrices(int productVariationId)
        {
            var config = _companyRepository.GetCompanySettingsAsync((this._identity.CurrentCompany ?? Guid.Empty), Core.Models.Identity.Companies.CompanySettingsType.VTexClientConfig).Result;
            this.vtexConfig = JsonConvert.DeserializeObject<LinxIOVTexClientConfig>(config.Value, JsonSettings.Settings);

            this._httpClient.BaseAddress = new Uri(vtexConfig.BaseUrl);
            this._httpClient.DefaultRequestHeaders.Add("x-vtex-api-appkey", this.vtexConfig.XvtexAppKey);
            this._httpClient.DefaultRequestHeaders.Add("x-vtex-api-apptoken", this.vtexConfig.XvtexAppToken);

            _logger.LogInformation("Requesting vtex-prices {0}", productVariationId);
            using (var client = new HttpClient())
            { // TODO: arrumar isso

                client.DefaultRequestHeaders.Add("x-vtex-api-appkey", this.vtexConfig.XvtexAppKey);
                client.DefaultRequestHeaders.Add("x-vtex-api-apptoken", this.vtexConfig.XvtexAppToken);

                var response = await client.GetAsync($"https://api.vtex.com/aramismenswear/pricing/prices/{productVariationId}");
                var responseString = await response.Content.ReadAsStringAsync();

                if (responseString.Contains("Price not found")) return null;

                try
                {
                    var responseSerialized = JsonConvert.DeserializeObject<VtexBasePrices>(responseString, JsonSettings.Settings);
                    this._logger.LogTrace("Response vtex-ids vtex-prices {0} {@response}", productVariationId, response);
                    return responseSerialized;
                }
                catch (Exception)
                {
                    _logger.LogError("Error on serialization {responseString}", responseString);

                    throw;
                }
            }
        }

    }

}