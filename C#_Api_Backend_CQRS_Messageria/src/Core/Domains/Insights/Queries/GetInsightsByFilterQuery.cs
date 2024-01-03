using System;
using Core.SeedWork;
using MediatR;

namespace Core.QuerysCommands.Queries.Insights
{
    public class GetInsightsByFilterQuery : IRequest<GetInsightsByFilterResponse>
    {
        public DateTime? BeginsAtInitialDate { get; set; }
        public DateTime? BeginsAtFinalDate { get; set; }
    }
}