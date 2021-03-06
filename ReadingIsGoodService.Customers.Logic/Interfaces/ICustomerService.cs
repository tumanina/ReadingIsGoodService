using ReadingIsGoodService.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Customers.Logic.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerModel>> GetCustomers();
        Task<int> CreateCustomer(CustomerModel customer, int userId);
    }
}
