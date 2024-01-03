using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface ISettingsRepository
    {
        Task<List<dynamic>> GetSpecifications(int typeId);
        Task<List<dynamic>> GetCategories();
    }
}