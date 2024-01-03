using System;
using Core.SeedWork;
using MediatR;

namespace Core.QuerysCommands.Queries.Orders.GetOrdersByFilter
{
    public class GetOrdersByFilterQuery : IPagedFilter, IRequest<GetOrdersByFilterResponse>
    {
        public int? Status { get; set; }
        public int? ShippingType { get; set; }
        public DateTime? OrderDateFrom { get; set; }
        public DateTime? OrderDateTo { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; } = "OrderedAt";
        public string OrderDirection { get; set; } = "DESC";
        public GetOrdersByFilterQuery()
        {
            PageIndex = 1;
            PageSize = 10;
        }
    }
}