using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadingIsGoodService.Common.Extensions;
using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Customers.Api.Controllers;
using ReadingIsGoodService.Customers.Logic.Interfaces;
using ReadingIsGoodService.CustomersApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadingIsGoodService.CustomersApi.Controllers
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
                return (await _customerService.GetCustomers()).Select(c => new CustomerDetailModel { Id = c.Id, Name = c.Name, Email = c.Email }).ToList();
            });
        }

        [HttpPost]
        public async Task<BaseApiDataModel<int>> CreateCustomer(CreateCustomerModel customer)
        {
            return await Execute(async () =>
            {
                return await _customerService.CreateCustomer(new CustomerModel { Name = customer.Name, Email = customer.Email },
                    userId: HttpContext.GetCurrentUserId());
            });
        }
    }
}
