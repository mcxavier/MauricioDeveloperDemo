using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Infra.ExternalServices.Reshop.Dtos
{
    [JsonObject(Title = "CustomerDataResult")]
    public class CustomerPermission
    {
        public CustomerPermission()
        {
            this.CustomFinalities = new List<CustomerFinality>();
        }

        public List<CustomerFinality> CustomFinalities;
    }

    [JsonObject(Title = "CustomerFinalityData")]
    public class CustomerFinality
    {
        public int FinalityId { get; set; }
        public string Term { get; set; }
        public bool Allow { get; set; }
        public string FinalityCode { get; set; }
        public string Finality { get; set; }
    }
}
