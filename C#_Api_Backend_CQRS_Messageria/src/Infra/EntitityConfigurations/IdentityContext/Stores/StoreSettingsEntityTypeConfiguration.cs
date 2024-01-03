using Core.Models.Identity.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.EntitityConfigurations.IdentityContext.Stores
{
    public class StoreSettingsEntityTypeConfiguration : IEntityTypeConfiguration<StoreSettings>
    {
        public void Configure(EntityTypeBuilder<StoreSettings> builder)
        {
            builder.ToTable("StoreSettings", "store");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Store).WithMany().HasForeignKey(x => x.StoreId);
        }
    }
}
