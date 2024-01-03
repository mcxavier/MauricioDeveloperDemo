using Core.Domains.Insights;
using Core.Models.Core.Insights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.EntitityConfigurations.CoreContext.Log
{
    public class InsightDataEntityTypeConfiguration : IEntityTypeConfiguration<InsightData>
    {
        public void Configure(EntityTypeBuilder<InsightData> builder)
        {
            builder.ToTable("InsightData", "insights");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.StoreCode).HasMaxLength(40);
            builder.HasOne(p => p.InsightType).WithMany().HasForeignKey(x => x.InsightTypeId);
            builder.Property(c => c.DecimalValue);
            builder.Property(c => c.IntValue);
            builder.Property(c => c.StringValue);
        }
    }
}
