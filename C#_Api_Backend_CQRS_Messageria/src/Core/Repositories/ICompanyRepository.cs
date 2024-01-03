using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models.Identity.Stores;
using Core.Models.Identity.Companies;
using Core.Models.Identity.Tenants;

namespace Core.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> GetCompanyByIdAsync(Guid companyId);
        Task<Tenant> GetCompanyByTenantIdAsync(Guid tenantId);
        Task<IList<Store>> GetCompanyStoresAsync(Guid companyId);
        Task<CompanySettings> GetCompanySettingsAsync(Guid companyId, CompanySettingsType type);
        Task<IList<CompanySettings>> ListCompanyIntegrateIOAsync(CompanySettingsType type);
    }
}