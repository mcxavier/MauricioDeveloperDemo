using System;

namespace Core.Dtos
{
    public class InsightDataDto
    {
        public string? Label { get; set; }
        public decimal? Value { get; set; }
        public string UnitLeft { get; set; }
        public string UnitRight { get; set; }
    }
}