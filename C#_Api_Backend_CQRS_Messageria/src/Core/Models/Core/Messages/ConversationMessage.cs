using System;
using Core.SeedWork;

namespace Core.Models.Core.Messages
{
    public class ConversationMessage : Entity<int>
    {
        public string Message { get; set; }
        public DateTime? SendAt { get; set; } = DateTime.Now;
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }
    }
}