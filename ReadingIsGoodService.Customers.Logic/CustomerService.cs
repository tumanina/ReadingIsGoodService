using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Customers.Logic.Interfaces;
using ReadingIsGoodService.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Customers.Logic
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<int> CreateCustomer(CustomerModel customer, int userId)
        {
            //todo: validation if email and names are specified + email validation
            return await _customerRepository.CreateCustomer(customer, userId);
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomers()
        {
            return await _customerRepository.GetCustomers();
        }
    }
}
