using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagedList.Core;

namespace Core.SeedWork
{
    public interface IGenericRepository<TEntity> : IUnitOfWork where TEntity : class
    {
        IQueryable<TEntity> Queryable();

        Task<IPagedList<TEntity>> PagedList(int pageNumber = 1, int pageRows = 20);

        Task<List<TEntity>> List();

        Task<TEntity> GetById(object id);

        void Insert(TEntity obj);

        void Update(TEntity obj);

        void Delete(object id);
    }
}