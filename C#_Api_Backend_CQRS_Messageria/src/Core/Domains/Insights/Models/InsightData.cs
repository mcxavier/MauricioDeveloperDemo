using System;
using System.Collections.Generic;
using Core.SeedWork;
using Core.SharedKernel;
using Core.Models.Core.Insights;

namespace Core.Domains.Insights
{
    public class InsightData : EntityWithMetadata<int>, IAggregateRoot, IStoreReferenced
    {
        public string StoreCode { get; set; }
        public int InsightTypeId { get; set; }
        public InsightDataType InsightType { get; set; }
        public decimal? DecimalValue { get; set; }
        public int? IntValue { get; set; }
        public string? StringValue { get; set; }
    }
}