using System.Threading.Tasks;
using Core.Domains.Catalogs.Repositories;
using Core.Models.Core.Catalogs;
using Infra.EntitityConfigurations.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly CoreContext _context;

        public CatalogRepository(CoreContext context)
        {
            _context = context;
        }

        public async Task<Catalog> GetCatalogById(int id)
        {
            return await _context.Catalogs.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}