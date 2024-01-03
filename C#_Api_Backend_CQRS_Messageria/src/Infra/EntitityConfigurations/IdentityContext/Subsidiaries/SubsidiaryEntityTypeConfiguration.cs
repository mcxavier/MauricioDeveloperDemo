using Core.Models.Identity.Subsidiaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.IdentityContext.Subsidiaries
{
    public class SubsidiaryEntityTypeConfiguration : IEntityTypeConfiguration<Subsidiary>
    {
        public void Configure(EntityTypeBuilder<Subsidiary> builder)
        {
            builder.ToTable("Subsidiaries", "subsidiary");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.EconomicGroup).WithMany().HasForeignKey(x => x.EconomicGroupId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}