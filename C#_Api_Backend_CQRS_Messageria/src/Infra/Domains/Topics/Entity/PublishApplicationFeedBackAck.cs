﻿namespace Infra.Domains.Topics
{
    public class PublishApplicationFeedBackAck
    {
        public string receiptId { get; set; }
        public string referenceMessageId { get; set; }
    }
}
