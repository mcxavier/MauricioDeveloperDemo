using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace CoreService.IntegrationsViewModels
{

    public class VtexProductVariation
    {

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("ProductId")]
        public int ProductId { get; set; }

        [JsonProperty("NameComplete")]
        public string NameComplete { get; set; }

        [JsonProperty("ProductName")]
        public string ProductName { get; set; }

        [JsonProperty("ProductDescription")]
        public string ProductDescription { get; set; }

        [JsonProperty("ProductRefId")]
        public string ProductRefId { get; set; }

        [JsonProperty("TaxCode")]
        public string TaxCode { get; set; }

        [JsonProperty("SkuName")]
        public string SkuName { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty("IsTransported")]
        public bool IsTransported { get; set; }

        [JsonProperty("IsInventoried")]
        public bool IsInventoried { get; set; }

        [JsonProperty("IsGiftCardRecharge")]
        public bool IsGiftCardRecharge { get; set; }

        [JsonProperty("ImageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("DetailUrl")]
        public string DetailUrl { get; set; }

        /*
        [JsonProperty("CSCIdentification")]
        public dynamic CscIdentification { get; set; }
        */

        [JsonProperty("BrandId")]
        public string BrandId { get; set; }

        [JsonProperty("BrandName")]
        public string BrandName { get; set; }

        /*
        [JsonProperty("Dimension")]
        public VtexProductVariation_Dimension Dimension { get; set; }
        */

        /*
        [JsonProperty("RealDimension")]
        public VtexProductVariation_RealDimension RealDimension { get; set; }
        */

        /*
        [JsonProperty("ManufacturerCode")]
        public dynamic ManufacturerCode { get; set; }
        */

        [JsonProperty("IsKit")]
        public bool IsKit { get; set; }

        /*
        [JsonProperty("KitItems")]
        public List<dynamic> KitItems { get; set; }

        [JsonProperty("Services")]
        public List<dynamic> Services { get; set; }

        [JsonProperty("Categories")]
        public List<dynamic> Categories { get; set; }

        [JsonProperty("Attachments")]
        public List<dynamic> Attachments { get; set; }

        [JsonProperty("Collections")]
        public List<dynamic> Collections { get; set; }
        */

        /*[JsonProperty("SkuSellers")]
        public List<VtexProductVariation_SkuSeller> SkuSellers { get; set; }

        [JsonProperty("SalesChannels")]
        public List<long> SalesChannels { get; set; }
        */

        [JsonProperty("Images")]
        public List<VtexProductVariationImage> Images { get; set; }

        [JsonProperty("SkuSpecifications")]
        public List<VtexProductVariationSpecification> SkuSpecifications { get; set; }

        [JsonProperty("ProductSpecifications")]
        public List<VtexProductVariationSpecification> ProductSpecifications { get; set; }


        [JsonProperty("ProductCategories")]
        public Dictionary<string, string> ProductCategories { get; set; }

        [JsonProperty("AlternateIds")]
        public VtexProductVariationAlternateIds AlternateIds { get; set; }

        [JsonProperty("AlternateIdValues")]
        public List<string> AlternateIdValues { get; set; }

        [JsonProperty("InformationSource")]
        public string InformationSource { get; set; }

        [JsonProperty("KeyWords")]
        public string KeyWords { get; set; }

        [JsonProperty("ReleaseDate")]
        public DateTimeOffset ReleaseDate { get; set; }

        [JsonProperty("ProductIsVisible")]
        public bool ProductIsVisible { get; set; }

        [JsonProperty("ShowIfNotAvailable")]
        public bool ShowIfNotAvailable { get; set; }

    }

    public class VtexProductVariationAlternateIds
    {

        [JsonProperty("Ean")]
        public string Ean { get; set; }

        [JsonProperty("RefId")]
        public string RefId { get; set; }

    }

    public class VtexProductVariationDimension
    {

        [JsonProperty("cubicweight")]
        public long Cubicweight { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

    }

    public class VtexProductVariationImage
    {

        [JsonProperty("ImageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("ImageName")]
        public string ImageName { get; set; }

        [JsonProperty("FileId")]
        public long FileId { get; set; }

    }

    public class VtexProductVariationSpecification
    {

        [JsonProperty("FieldId")]
        public long FieldId { get; set; }

        [JsonProperty("FieldName")]
        public string FieldName { get; set; }

        [JsonProperty("FieldValueIds")]
        public List<long> FieldValueIds { get; set; }

        [JsonProperty("FieldValues")]
        public List<string> FieldValues { get; set; }

        [JsonProperty("IsFilter")]
        public bool IsFilter { get; set; }

        [JsonProperty("FieldGroupId")]
        public long FieldGroupId { get; set; }

        [JsonProperty("FieldGroupName")]
        public string FieldGroupName { get; set; }

    }

    public class VtexProductVariationRealDimension
    {

        [JsonProperty("realCubicWeight")]
        public decimal RealCubicWeight { get; set; }

        [JsonProperty("realHeight")]
        public long RealHeight { get; set; }

        [JsonProperty("realLength")]
        public long RealLength { get; set; }

        [JsonProperty("realWeight")]
        public long RealWeight { get; set; }

        [JsonProperty("realWidth")]
        public long RealWidth { get; set; }

    }

    public class VtexProductVariationSkuSeller
    {

        [JsonProperty("SellerId")]
        public string SellerId { get; set; }

        [JsonProperty("StockKeepingUnitId")]
        public long StockKeepingUnitId { get; set; }

        [JsonProperty("SellerStockKeepingUnitId")]
        public string SellerStockKeepingUnitId { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty("FreightCommissionPercentage")]
        public long FreightCommissionPercentage { get; set; }

        [JsonProperty("ProductCommissionPercentage")]
        public long ProductCommissionPercentage { get; set; }

    }

}