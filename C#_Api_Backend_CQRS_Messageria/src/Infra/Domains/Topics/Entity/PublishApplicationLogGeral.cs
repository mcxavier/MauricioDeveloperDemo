using Core.SharedKernel;
using Infra.QueryCommands.Commands.Topics;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.Domains.Topics
{
    public class PublishApplicationLogGeral : SkuEntityWithMetadata<int>, IRequest<Response>
    {
        public string StoreCode { get; set; }
        public string TopicId { get; set; }
        public string EntityId { get; set; }
        public string ReferenceMessageId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string MessageJson { get; set; }
        public DateTime DataHora { get; set; }
    }
}
