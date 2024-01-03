using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos
{

    public class LinxUxItemExtensionAttributes
    {

        [JsonProperty("ean")]
        public string Ean { get; set; }

        [JsonProperty("ncm")]
        public string Ncm { get; set; }

        [JsonProperty("origem")]
        public int Origem { get; set; }

        [JsonProperty("vtexSku")]
        public string VtexSku { get; set; }

        [JsonProperty("isFreeGift")]
        public bool IsFreeGift { get; set; }

        [JsonProperty("vtexListPrice")]
        public int VtexListPrice { get; set; }

        [JsonProperty("measurementUnit")]
        public string MeasurementUnit { get; set; }

        [JsonProperty("warehouseExternal")]
        public int WarehouseExternal { get; set; }

    }

}