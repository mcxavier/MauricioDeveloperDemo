using Core.Domains.Catalogs.Queries;
using Infra.Domains.Catalogs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infra.Responses;
using Core.SharedKernel;
using Core.Domains.Catalogs.Requests;

namespace Infra.Domains.Catalogs
{
    public interface ICatalogService
    {
        public Task<FilterCatalogResponse> GetCatalogsResume(FilterCatalogQuery catalogQuery, string storeCode);
        public Task<CatalogDetailsResponse> GetCatalogDetailsById(int id);
        public Task<CatalogProductsResponse> GetCatalogById(int id);
        public List<CatalogProductListingResponse> GetCatalogProductsById(int id, out PagerInfo pagerInfo, int pageIndex, int pageSize);
        public Task<Response> PostCatalogCustomerReceived(int ReceivedCatalogId, int ReceivedCustomerId);
        public Task<Response> GetCatalogProducts(string term, int pageIndex, int pageSize);
        public Task<Response> GetAllProducts(ShowcaseListingFilterRequest filter);
        public Task<Response> CreateCatalog(CreateCatalogRequest request, string storePortal, int? id = null);
        public Task<Response> GetCatalogsItensInfos(int id);
    }
}