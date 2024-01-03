using System;

using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos.PagarMe
{

    public class PagarMeOrderCustomer
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("addresses")]
        public PagarMeAddresses Addresses { get; set; }

        [JsonProperty("documents")]
        public PagarMeDocument[] Documents { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("telephones")]
        public PagarMeTelephone[] Telephones { get; set; }

        [JsonProperty("socialNumber")]
        public string SocialNumber { get; set; }

    }

}