using System.Collections.Generic;


namespace Infra.QueryCommands.Commands.Topics
{
    public class ExtendedSkuEntityWithMetadata<T> : SkuEntityWithMetadata<T>
    {
        public MetricEntity? Height { get; set; }
        public MetricEntity? Length { get; set; }
        public bool? RequiresShipping { get; set; }
        public string ProductId { get; set; }
        public string Title { get; set; }
        public bool Enabled { get; set; }
        public IList<DataIdNameEntity?>? VariationOptions { get; set; }
        public MetricEntity? Weight { get; set; }
        public MetricEntity? Width { get; set; }
    }

}
