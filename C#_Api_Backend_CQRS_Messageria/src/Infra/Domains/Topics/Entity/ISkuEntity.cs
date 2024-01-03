using Core.SeedWork;

namespace Infra.QueryCommands.Commands.Topics
{
    public abstract class ISkuEntity : Entity<int>
    {
        public string Sku { get; set; }
        public string SkuId { get; set; }

    }

}
    