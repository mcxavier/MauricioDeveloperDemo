using Core.Dtos;
using PagedList.Core;

namespace Core.QuerysCommands.Queries.Customers.GetCustomersByFilter
{
    public class GetCustomersByFilterResponse
    {
        public IPagedList<CustomerResumeDto> Customers { get; set; }
    }
}