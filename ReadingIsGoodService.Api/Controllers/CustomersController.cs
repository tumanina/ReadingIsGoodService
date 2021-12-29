using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadingIsGoodService.Api.Models;
using ReadingIsGoodService.Logic.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Api.Controllers
{
    [ApiController]
    [Route("api/v1/customers")]
    public class CustomersController : BaseController
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<BaseApiDataModel<List<CustomerDetailModel>>> GetCustomers()
        {
            return await Execute(async () =>
            {
                return (await _customerService.GetCustomers()).Select(c => new CustomerDetailModel { Id = c.Id, Name = c.Name }).ToList();
            });
        }

        [HttpPost]
        public async Task<BaseApiDataModel<int>> CreateCustomer(CreateCustomerModel customer)
        {
            return await Execute(async () =>
            {
                return await _customerService.CreateCustomer(new Common.Models.CustomerModel { Name = customer.Name, Email = customer.Email }, 5);
            });
        }
    }
}
