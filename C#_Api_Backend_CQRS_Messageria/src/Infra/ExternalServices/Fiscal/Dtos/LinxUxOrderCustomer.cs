using System;

using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos
{

    public class LinxUxOrderCustomer
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
        public LinxUxAddresses Addresses { get; set; }

        [JsonProperty("documents")]
        public LinxUxDocument[] Documents { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("telephones")]
        public LinxUxTelephone[] Telephones { get; set; }

        [JsonProperty("socialNumber")]
        public string SocialNumber { get; set; }

    }

}