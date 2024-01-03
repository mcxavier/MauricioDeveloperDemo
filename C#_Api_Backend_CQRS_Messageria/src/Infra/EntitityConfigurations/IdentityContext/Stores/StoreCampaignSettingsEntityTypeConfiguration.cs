using Core.Models.Identity.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.IdentityContext.Stores
{
    public class StoreCampaignSettingsEntityTypeConfiguration : IEntityTypeConfiguration<StoreCampaignSettings>
    {
        public void Configure(EntityTypeBuilder<StoreCampaignSettings> builder)
        {
            builder.ToTable("CampaignSettings", "store");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Store).WithMany().HasForeignKey(x => x.StoreId);
        }
    }
}