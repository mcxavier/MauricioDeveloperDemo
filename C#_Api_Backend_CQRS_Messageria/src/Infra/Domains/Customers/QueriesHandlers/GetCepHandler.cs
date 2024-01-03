using Core.Domains.Customers.Dtos;
using Core.Repositories;
using Core.SharedKernel;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace Infra.Domains.Customers.QueriesHandlers
{
    public class GetCepHandler : IRequestHandler<CepDto, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<GetCepHandler> _logger;
        private readonly SmartSalesIdentity _identity;
        private readonly ICompanyRepository _companyRepository;

        public GetCepHandler(CoreContext context, SmartSalesIdentity identity, ICompanyRepository companyRepository, ILogger<GetCepHandler> logger)
        {
            _context = context;
            _identity = identity;
            _companyRepository = companyRepository;
            _logger = logger;
        }

        public async Task<Response> Handle(CepDto request, CancellationToken cancellationToken)
        {
            var cep = new CepCompleto();

            using (var client = new HttpClient())
            {
                try
                {
                    var config = _companyRepository.GetCompanySettingsAsync((this._identity.CurrentCompany ?? Guid.Empty), Core.Models.Identity.Companies.CompanySettingsType.LinxCepConfig).Result;
                    var auth = JsonConvert.DeserializeObject<Authentication>(config.Value, JsonSettings.Settings);

                    client.BaseAddress = new Uri(auth.baseAddress);

                    var httpContent = new StringContent(config.Value, Encoding.UTF8, "application/json");

                    var httpResponseMessage = await client.PostAsync("/v1/auth", httpContent);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        var authString = await httpResponseMessage.Content.ReadAsStringAsync();
                        AuthToken authRet = JsonConvert.DeserializeObject<AuthToken>(authString);

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authRet.token);

                        httpResponseMessage = await client.GetAsync("/v1/cep/" + request.numCep.Replace("-", ""));

                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            authString = await httpResponseMessage.Content.ReadAsStringAsync();
                            cep = JsonConvert.DeserializeObject<CepCompleto>(authString);
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogCritical("Error in sending data {@exception}", ex);

                    return new Response("Cep Inválido: " + ex.Message, true, cep);
                }
                finally
                {
                    client.Dispose();
                }

                return new Response("Sucesso", false, cep);
            }
        }

        class Authentication
        {
            public string baseAddress { get; set; }
            public string name { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string email { get; set; }
        }

        class AuthToken
        {
            public string exp { get; set; }
            public string message { get; set; }
            public string token { get; set; }
        }

        class Cep
        {
            public string ativo { get; set; }
            public string bairro { get; set; }
            public string baseatualizadaem { get; set; }
            public string cep { get; set; }
            public string cidade { get; set; }
            public string ibge { get; set; }
            public string idparidade { get; set; }
            public string infadicional { get; set; }
            public string logradourooficial { get; set; }
            public string logradourorecomendado { get; set; }
            public string nomecomplemento1 { get; set; }
            public string nomecomplemento2 { get; set; }
            public string numerocomplemento1 { get; set; }
            public string numerocomplemento2 { get; set; }
            public string preposicao { get; set; }
            public string tipologradouro { get; set; }
            public string trechofinal { get; set; }
            public string trechoinicial { get; set; }
            public string uf { get; set; }
        }

        class CepCompleto
        {
            public Cep linxcep { get; set; }
        }
    }
}
