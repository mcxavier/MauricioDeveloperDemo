using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repositories;
using Dapper;
using Infra.DomainInfra.Catalogs.Sql;
using Infra.EntitityConfigurations.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly CoreContext _context;

        public SettingsRepository(CoreContext context)
        {
            _context = context;
        }

        public async Task<List<dynamic>> GetCategories()
        {
            var result = await this._context.Database.GetDbConnection().QueryAsync(SpecificationsSql.GetCategories);
            return result.ToList();
        }

        public async Task<List<dynamic>> GetSpecifications(int typeId)
        {
            var result = await this._context.Database.GetDbConnection().QueryAsync(SpecificationsSql.GetSpecifications, new { typeId });
            return result.ToList();
        }
    }
}
