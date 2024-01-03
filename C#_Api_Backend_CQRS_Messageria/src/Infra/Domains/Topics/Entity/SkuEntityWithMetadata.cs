using Core.SeedWork;

namespace Infra.QueryCommands.Commands.Topics
{
    public abstract class SkuEntityWithMetadata<T> : EntityWithMetadata<T>
    {
        public string Sku { get; set; }
        public string SkuId { get; set; }
    }
}
