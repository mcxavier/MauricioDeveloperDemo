using System;
using System.Collections.Generic;
using Core.SeedWork;
using MediatR;

namespace Core.QuerysCommands.Queries.Customers.GetCustomersByFilter
{
    public class GetCustomersByFilterQuery : IRequest<GetCustomersByFilterResponse>, IPagedFilter
    {
        public decimal AverageTicketStart { get; set; }
        public decimal AverageTicketEnd { get; set; }
        public DateTime? BuyPeriodInitialDate { get; set; }
        public DateTime BuyPeriodFinalDate { get; set; } = DateTime.Now;
        public DateTime? NotBuyPeriodInitialDate { get; set; }
        public DateTime NotBuyPeriodFinalDate { get; set; } = DateTime.Now;
        public bool OnlyValidPhone { get; set; } = true;
        public string CustomerName { get; set; }
        public string CustomerDocument { get; set; }
        public IEnumerable<int> Categories { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? Seller { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int AgeMin { get; set; } = 0;
        public int AgeMax { get; set; } = 100;
        public GetCustomersByFilterQuery()
        {
            Categories = new List<int>();
        }
    }
}