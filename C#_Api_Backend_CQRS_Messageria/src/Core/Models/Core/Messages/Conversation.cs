using System.Collections.Generic;
using Core.Models.Core.Customers;
using Core.SeedWork;
using Core.SharedKernel;

namespace Core.Models.Core.Messages
{
    public class Conversation : EntityWithMetadata<int>, IAggregateRoot, IStoreReferenced
    {
        public string StoreCode { get; set; }
        public int CustomerId { get; set; }
        public string IntegrationId { get; set; }
        public Customer Customer { get; set; }
        public IList<ConversationMessage> Messages { get; set; }
    }
}