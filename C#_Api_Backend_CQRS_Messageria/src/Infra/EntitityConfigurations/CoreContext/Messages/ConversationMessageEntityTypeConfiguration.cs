using Core.Models.Core.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Messages
{
    public class ConversationMessageEntityTypeConfiguration : IEntityTypeConfiguration<ConversationMessage>
    {
        public void Configure(EntityTypeBuilder<ConversationMessage> builder)
        {
            builder.ToTable("Messages", "conversation");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(p => p.Conversation).WithMany(x => x.Messages).HasForeignKey(x => x.ConversationId);
        }
    }
}