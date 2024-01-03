using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models.Identity.Stores;
using Core.Models.Identity.Companies;
using Core.Models.Identity.Tenants;
using Core.Repositories;
using Infra.EntitityConfigurations.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infra.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IdentityContext _context;

        public CompanyRepository(IdentityContext context)
        {
            _context = context;
        }

        public async Task<Company> GetCompanyByIdAsync(Guid companyId)
        {
            return await _context.Companies.FirstOrDefaultAsync(x => x.Id == companyId);
        }

        public Task<Tenant> GetCompanyByTenantIdAsync(Guid tenantId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Store>> GetCompanyStoresAsync(Guid companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<CompanySettings> GetCompanySettingsAsync(Guid companyId, CompanySettingsType type)
        {
            return await _context.CompanySettings.FirstOrDefaultAsync(x => x.CompanyId == companyId && x.Type.Id == type.Id);
        }

        public async Task<IList<CompanySettings>> ListCompanyIntegrateIOAsync(CompanySettingsType type)
        {
            return await _context.CompanySettings.Where(x => x.Type.Id == type.Id).ToListAsync();
        }
    }
}