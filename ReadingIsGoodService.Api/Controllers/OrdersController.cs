using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadingIsGoodService.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class OrdersController : BaseController
    {
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("orders")]
        [Route("customers/{customerId}/orders")]
        public async Task<BaseApiDataModel<List<OrderDetailModel>>> GetCustomerOrders(int customerId)
        {
            return await Execute(async () =>
            {
                return new List<OrderDetailModel>();
            });
        }

        [HttpGet]
        [Route("orders/{id}")]
        public async Task<BaseApiDataModel<OrderDetailModel>> GetOrders(int id)
        {
            return await Execute(async () =>
            {
                return new OrderDetailModel();
            });
        }

        [HttpPost]
        [Route("orders")]
        public async Task<BaseApiDataModel<OrderDetailModel>> CreateOrder()
        {
            return await Execute(async () =>
            {
                return new OrderDetailModel();
            });
        }
    }
}
