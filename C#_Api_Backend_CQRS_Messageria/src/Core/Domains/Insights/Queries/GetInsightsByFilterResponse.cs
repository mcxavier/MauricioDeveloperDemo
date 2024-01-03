using Core.Dtos;
using Core.SharedKernel;
using PagedList.Core;
using System.Collections.Generic;

namespace Core.QuerysCommands.Queries.Insights
{
    public class GetInsightsByFilterResponse
    {
        public IList<InsightsResumeDto> Insights { get; set; }
    }
}