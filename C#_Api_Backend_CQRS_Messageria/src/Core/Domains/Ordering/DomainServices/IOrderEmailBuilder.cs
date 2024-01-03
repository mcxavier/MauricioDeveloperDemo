using Core.Domains.Ordering.Models;

namespace Core.Domains.Ordering.DomainServices
{
    public interface IOrderEmailBuilder
    {
        string BuildEmailBody(Order order);
    }
}