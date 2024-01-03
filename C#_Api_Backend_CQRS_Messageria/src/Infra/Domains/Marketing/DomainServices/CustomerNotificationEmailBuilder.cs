using Core.Domains.Marketing.DomainServices;
using Core.Models.Core.Products;

namespace Infra.Domains.Marketing.DomainServices
{
    public class CustomerNotificationEmailBuilder : ICustomerNotificationEmailBuilder
    {
        public string BuildEmailBody(string customerName, ProductVariation variation)
        {
            throw new System.NotImplementedException();
        }
    }
}