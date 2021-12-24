using ReadingIsGoodService.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Data.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerModel>> GetCustomers();
        Task<int> CreateCustomer(CustomerModel customer);
    }
}
