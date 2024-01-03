using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Core.SharedKernel;
using Newtonsoft.Json;
using Core.SeedWork;

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
