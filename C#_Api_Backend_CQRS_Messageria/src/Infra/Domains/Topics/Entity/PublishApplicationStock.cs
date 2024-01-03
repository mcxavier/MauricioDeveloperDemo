using MediatR;
using System;
using Core.SharedKernel;

namespace Infra.QueryCommands.Commands.Topics
{
    public class PublishApplicationStock : SkuEntityWithMetadata<int>, IRequest<Response>
    {
        public int AvailableQuantity { get; set; }
        public bool Enabled { get; set; }
        public string LocationId { get; set; }
        public string StockType { get; set; }
        public int TotalQuantity { get; set; }
        public DateTime UpdatedAt { get; set; }
    }  
}
