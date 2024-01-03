using Core.Models.Identity.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.IdentityContext.Companies
{
    public class CompanySettingsEntityTypeConfiguration : IEntityTypeConfiguration<CompanySettings>
    {
        public void Configure(EntityTypeBuilder<CompanySettings> builder)
        {
            builder.ToTable("CompanySettings", "company");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);
        }
    }
}