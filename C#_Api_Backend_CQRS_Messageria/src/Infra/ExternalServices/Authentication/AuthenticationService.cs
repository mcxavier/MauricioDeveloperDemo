using System;

using Infra.ExternalServices.Authentication.Dtos;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Infra.ExternalServices.Authentication
{

    public class AuthenticationService : IAuthenticationService
    {

        private readonly IAuthenticationApi _authenticationApi;

        public AuthenticationService(IAuthenticationApi authenticationApi)
        {
            _authenticationApi = authenticationApi;
        }

        public async Task<AuthenticationDto> AuthenticateAsync(string username, string password)
        {
            try {
                using (var client = new HttpClient()) {
                    var jsonData = await client.GetFromJsonAsync<AuthenticateDto>(_authenticationApi.AuthenticateJson(username, password));

                    return new AuthenticationDto{
                        CurrentCompany     = jsonData.AuthenticateJsonResult[0].Value,
                        AuthorizationToken = jsonData.AuthenticateJsonResult[1].Value,
                        CurrentUser        = jsonData.AuthenticateJsonResult[2].Value,
                        AccessGroup        = jsonData.AuthenticateJsonResult[3].Value,
                        EconomicGroup      = jsonData.AuthenticateJsonResult[4].Value,
                        Environment        = jsonData.AuthenticateJsonResult[5].Value,
                        UserId             = jsonData.AuthenticateJsonResult[6].Value,
                        Application        = jsonData.AuthenticateJsonResult[7].Value,
                        ServiceBusUrl      = jsonData.AuthenticateJsonResult[8].Value,
                    };
                }
            } catch (Exception e) {
                Console.WriteLine(e);

                throw;
            }
        }

    }

}