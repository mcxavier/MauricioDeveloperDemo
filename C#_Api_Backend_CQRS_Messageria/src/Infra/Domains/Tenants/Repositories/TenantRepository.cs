using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models.Identity.Tenants;
using Core.Repositories;
using Infra.EntitityConfigurations.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{

    public class TenantRepository : ITenantRepository
    {

        private readonly IdentityContext _context;

        public TenantRepository(IdentityContext context)
        {
            _context = context;
        }

        public async Task<Tenant> GetTenantByCompanyIdAsync(Guid companyId)
        {
            return await _context.Tenants.FirstOrDefaultAsync(tenant => tenant.CompanyId == companyId);
        }

        public async Task<Tenant> GetTenantByNameAsync(string companyName)
        {
            return await _context.Tenants.FirstOrDefaultAsync(tenant => tenant.Name.ToLower() == companyName.ToLower());
        }

        public async Task<IList<Tenant>> GetAllTenants()
        {
            return await _context.Tenants.ToListAsync();
        }
    }
}