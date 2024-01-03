using Core.Models.Identity.Stores;
using Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.EntitityConfigurations.IdentityContext.Stores
{
    public class StoreSettingsTypeEntityTypeConfiguration : IEntityTypeConfiguration<StoreSettingsType>
    {
        public void Configure(EntityTypeBuilder<StoreSettingsType> builder)
        {
            builder.ToTable("StoreSettingsType", "store");
            builder.HasKey(x => x.Id);
            builder.HasData(Enumeration.GetAll<StoreSettingsType>());
        }
    }
}
