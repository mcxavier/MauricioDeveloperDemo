using System.Threading.Tasks;

namespace Core.SeedWork
{
    public interface IUnitOfWork
    {
        Task<int> Save();
    }
}