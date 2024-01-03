using System;
using System.Security.Principal;

namespace Infra.ExternalServices.Authentication
{
    public class SmartSalesIdentity : IIdentity
    {
        public string CurrentCompanyName { get; set; }

        public Guid? CurrentCompany { get; set; }

        public Guid? AuthorizationToken { get; set; }

        public Guid? CurrentUser { get; set; }

        public Guid? EconomicGroup { get; set; }

        public string CurrentStorePortal { get; set; }

        public string CurrentStoreCode { get; set; }

        public string CurrentStoreName { get; set; }

        public Guid? CurrentStoreId { get; set; }

        public bool IsCustomer { get; set; }

        public string AuthenticationType { get; set; }

        public string Name { get; set; }

        public bool IsAuthenticated { get; set; }

        public string LinxIOApplicationId { get; set; }

        public string LinxIOUsername { get; set; }

        public string LinxIOPassword { get; set; }
    }
}