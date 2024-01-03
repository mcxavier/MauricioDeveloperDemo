﻿using MediatR;
using System;
using System.Collections.Generic;
using Core.SharedKernel;

namespace Infra.QueryCommands.Commands.Topics
{
    public class PublishApplicationPrice : SkuEntityWithMetadata<int>, IRequest<Response>
    {
        public IList<string?>? Barcode { get; set; }
        public decimal ListPrice { get; set; }
        public string? PriceListId { get; set; }
        public decimal SalesPrice { get; set; }
        public DateTime? SalesPriceFrom { get; set; }
        public DateTime? SalesPriceTo { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
