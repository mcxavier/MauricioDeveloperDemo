using Core.Models.Identity.EconomicGroups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.IdentityContext.EconomicGroups
{
    public class EconomicGroupEntityTypeConfiguration : IEntityTypeConfiguration<EconomicGroup>
    {
        public void Configure(EntityTypeBuilder<EconomicGroup> builder)
        {
            builder.ToTable("EconomicGroups", "economicGroup");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);
        }
    }
}