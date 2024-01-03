using System;
using System.Collections.Generic;

namespace Core.Dtos
{
    public class InsightsResumeDto
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string TotalValue { get; set; }
        public string UnitLeft { get; set; }
        public string UnitRight { get; set; }
        public IList<InsightDataDto> Data { get; set; }
        public IList<List<InsightDataDto>> DataSeries { get; set; }
    }
}