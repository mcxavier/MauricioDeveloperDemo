using Core.Domains.Catalogs.Dtos;
using Core.SharedKernel;
using System.Collections.Generic;

namespace Infra.Domains.Catalogs.Responses
{
    public class FilterCatalogResponse
    {
        public IEnumerable<CatalogResumeDto> filteredCatalogs;
        public PagerInfo pagerInfo;
    }
}