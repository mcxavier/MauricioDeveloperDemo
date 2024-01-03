using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Core.Models.Identity.Stores;

namespace Core.Repositories
{
    public interface IStoreRepository
    {
        Task<Store> GetStoreByIdAsync(Guid storeId);

        Task<Store> GetStoreByStoreCodeAsync(string storeCode);

        Task<IList<Store>> GetAllStoresAsync(Guid companyId);

        Task<Store> GetStoreByPortalNameAsync(string portalName);

        Task<StoreCampaignSettings> GetStoreCampaignSettingsAsync(Guid storeId);

        Task<StoreGatewaySettings> GetStoreGatewaySettingsAsync(Guid storeId);

        Task<StoreEmailSettings> GetStoreEmailSettingsAsync(Guid storeId);

        Task<StoreAddress> GetStoreAddressAsync(Guid storeId);

        Task<StoreAddress> GetStoreAddressAsync(string storeCode);

        Task<StoreErpSettings> GetStoreErpSettingsAsync(Guid storeId);

        Task<StoreSettings> GetStoreSettingsAsync(Guid storeId, StoreSettingsType type);
    }
}