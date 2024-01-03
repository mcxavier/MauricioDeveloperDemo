using Core.Models.Identity.Companies;
using Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.IdentityContext.Companies
{
    public class CompanySettingsTypeEntityTypeConfiguration : IEntityTypeConfiguration<CompanySettingsType>
    {
        public void Configure(EntityTypeBuilder<CompanySettingsType> builder)
        {
            builder.ToTable("CompanySettingsType", "company");
            builder.HasKey(x => x.Id);
            builder.HasData(Enumeration.GetAll<CompanySettingsType>());
        }
    }
}