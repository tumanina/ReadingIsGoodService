using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadingIsGoodService.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Api.Controllers
{
    [ApiController]
    [Route("api/v1/customers")]
    public class CustomersController : BaseController
    {
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ILogger<CustomersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<BaseApiDataModel<List<CustomerDetailModel>>> GetCustomers()
        {
            return await Execute(async () =>
            {
                return new List<CustomerDetailModel>();
            });
        }

        [HttpPost]
        public async Task<BaseApiDataModel<CustomerDetailModel>> CreateCustomer(CreateCustomerModel customer)
        {
            return await Execute(async () =>
            {
                return new CustomerDetailModel();
            });
        }
    }
}
