using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models.Core.Customers;

namespace Core.Domains.Customers.Repositories
{

    public interface ICustomersRepository
    {
        Task<IEnumerable<Customer>> GetCustomers();
        Task<Customer> GetCustomerByEmail(string email);
    }
}