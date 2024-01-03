using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models.Identity.Tenants;

namespace Core.Repositories
{
    public interface ITenantRepository
    {
        Task<Tenant> GetTenantByCompanyIdAsync(Guid companyId);

        Task<Tenant> GetTenantByNameAsync(string companyName);

        Task<IList<Tenant>> GetAllTenants();
    }
}