using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Identity.Stores;
using Core.Repositories;
using Infra.EntitityConfigurations.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly IdentityContext _context;

        public StoreRepository(IdentityContext context)
        {
            _context = context;
        }

        public async Task<Store> GetStoreByIdAsync(Guid storeId)
        {
            return await _context.Stores.FirstOrDefaultAsync(x => x.Id == storeId);
        }

        public async Task<Store> GetStoreByStoreCodeAsync(string storeCode)
        {
            return await _context.Stores.FirstOrDefaultAsync(x => x.StoreCode == storeCode);
        }

        public async Task<IList<Store>> GetAllStoresAsync(Guid companyId)
        {
            var result = await _context.Stores.Where(x => x.CompanyId == companyId).ToListAsync();
            return result?.Where(x => x.IsActive).ToList();
        }

        public async Task<Store> GetStoreByPortalNameAsync(string portalName)
        {
            return await _context.Stores.FirstOrDefaultAsync(x => x.PortalUrl == portalName);
        }

        public async Task<StoreCampaignSettings> GetStoreCampaignSettingsAsync(Guid storeId)
        {
            return await _context.StoreCampaignSettings.FirstOrDefaultAsync(x => x.StoreId == storeId);
        }

        public async Task<StoreGatewaySettings> GetStoreGatewaySettingsAsync(Guid storeId)
        {
            return await _context.StoreGatewaySettings.FirstOrDefaultAsync(x => x.StoreId == storeId);
        }

        public async Task<StoreEmailSettings> GetStoreEmailSettingsAsync(Guid storeId)
        {
            return await _context.StoreEmailSettings.FirstOrDefaultAsync(x => x.StoreId == storeId);
        }

        public async Task<StoreAddress> GetStoreAddressAsync(Guid storeId)
        {
            return await _context.StoreAddresses.FirstOrDefaultAsync(x => x.StoreId == storeId);
        }

        public async Task<StoreAddress> GetStoreAddressAsync(string storeCode)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(x => x.StoreCode == storeCode);
            return await _context.StoreAddresses.FirstOrDefaultAsync(x => x.StoreId == store.Id);
        }

        public async Task<StoreErpSettings> GetStoreErpSettingsAsync(Guid storeId)
        {
            return await _context.StoreErpSettings.FirstOrDefaultAsync(x => x.StoreId == storeId);
        }

        public async Task<StoreSettings> GetStoreSettingsAsync(Guid storeId, StoreSettingsType type)
        {
            return await _context.StoreSettings.FirstOrDefaultAsync(x => x.StoreId == storeId && x.Type.Id == type.Id);
        }

    }
}