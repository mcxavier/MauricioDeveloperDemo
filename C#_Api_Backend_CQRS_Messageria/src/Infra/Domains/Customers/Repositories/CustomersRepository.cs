using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domains.Customers.Repositories;
using Core.Models.Core.Customers;
using Infra.EntitityConfigurations.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.QueryCommands.Queries.Customers
{

    public class CustomersRepository : ICustomersRepository
    {

        private readonly CoreContext _context;

        public CustomersRepository(CoreContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Customer>> GetCustomers()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Customer> GetCustomerByEmail(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Email.Contains(email));
        }
    }
}