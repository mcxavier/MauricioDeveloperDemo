using Core.Domains.Ordering.Dtos;
using Core.Dtos;
using PagedList.Core;

namespace Core.QuerysCommands.Queries.Orders.GetOrdersByFilter
{
    public class GetOrdersByFilterResponse
    {
        public IPagedList<OrderResumeDto> Orders { get; }

        public GetOrdersByFilterResponse(IPagedList<OrderResumeDto> orders)
        {
            Orders = orders;
        }
    }
}