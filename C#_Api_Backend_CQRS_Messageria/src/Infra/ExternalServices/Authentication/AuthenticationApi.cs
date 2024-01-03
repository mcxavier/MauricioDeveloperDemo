using System;

using Core.Repositories;

using Microsoft.Extensions.Configuration;

namespace Infra.ExternalServices.Authentication
{

    public class AuthenticationApi : IAuthenticationApi
    {

        private readonly SmartSalesIdentity _identity;
        private readonly IStoreRepository      _storeRepository;

        public AuthenticationApi(SmartSalesIdentity identity,
                                 IStoreRepository storeRepository) {
            
            this._storeRepository = storeRepository;
            this._identity = identity;
        }

        public string AuthenticateJson(string username, string password)
        {
            var erp = _storeRepository.GetStoreErpSettingsAsync(_identity.CurrentStoreId ?? Guid.Empty).Result;

            var host = erp.ServiceHost;
            var environment = erp.Environment;

            return $"{host}/{environment}/Linx-Framework-BV-Autorizacao-AutorizacaoDomainService.svc/json/AuthenticateJson?userName={username}&password={password}&applicationId=00000000-0000-0000-0000-000000000000";
        }

    }

}