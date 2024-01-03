using Core.Models.Core.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Messages
{
    public class ConversationEntityTypeConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToTable("Conversations", "conversation");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(p => p.Customer).WithMany().HasForeignKey(x => x.CustomerId);
            builder.HasMany(p => p.Messages).WithOne(x => x.Conversation).HasForeignKey(x => x.ConversationId);
        }
    }
}