using System.Threading.Tasks;
using Core.Models.Core.Catalogs;

namespace Core.Domains.Catalogs.Repositories
{
    public interface ICatalogRepository
    {
        Task<Catalog> GetCatalogById(int id);
    }
}