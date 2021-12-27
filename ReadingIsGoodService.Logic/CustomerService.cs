using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Data.Repositories;
using ReadingIsGoodService.Logic.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Logic
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<int> CreateCustomer(CustomerModel customer)
        {
            //todo: validation if email and names are specified + email validation
            return await _customerRepository.CreateCustomer(customer);
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomers()
        {
            return await _customerRepository.GetCustomers();
        }
    }
}
