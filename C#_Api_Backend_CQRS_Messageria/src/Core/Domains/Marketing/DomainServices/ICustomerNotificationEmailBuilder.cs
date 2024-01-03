using Core.Models.Core.Products;

namespace Core.Domains.Marketing.DomainServices
{
    public interface ICustomerNotificationEmailBuilder
    {
        string BuildEmailBody(string customerName, ProductVariation variation);
    }
}