using System;
using System.Threading.Tasks;
using Core.Repositories;
using Infra.ExternalServices.Authentication;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Utils;

namespace Infra.ExternalServices.Chatting
{
    public class TwilioService : IMessageService
    {
        private readonly IConfiguration _configuration;
        private readonly SmartSalesIdentity _identity;
        private readonly ICompanyRepository _companyRepository;

        public TwilioService(IConfiguration configuration,
                                      SmartSalesIdentity identity,
                                      ICompanyRepository companyRepository)
        {
            this._configuration = configuration;
            this._identity = identity;
            this._companyRepository = companyRepository;
        }

        public async Task<string> Send(string from, string to, string message)
        {
            var config = _companyRepository.GetCompanySettingsAsync((this._identity.CurrentCompany ?? Guid.Empty), Core.Models.Identity.Companies.CompanySettingsType.TwilioConfig).Result;
            var twilioConfig = JsonConvert.DeserializeObject<LinxIOTwilioConfig>(config.Value, JsonSettings.Settings);

            var accountSid = "";
            var authToken = "";

            if (twilioConfig != null)
            {
                accountSid = twilioConfig.AccountSid;
                authToken = twilioConfig.AuthToken;
            }


            TwilioClient.Init(accountSid, authToken);

            var messageResource = await MessageResource.CreateAsync(
                body: message,
                @from: new Twilio.Types.PhoneNumber(string.Concat("whatsapp:+", from)),
                to: new Twilio.Types.PhoneNumber(string.Concat("whatsapp:+", to))
            );

          return messageResource.Sid;
        }

    }


    public class LinxIOTwilioConfig 
    {
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
    }

}
