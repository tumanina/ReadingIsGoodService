using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadingIsGoodService.Api.Models;
using ReadingIsGoodService.Logic.Interfaces;
using ReadingIsGoodService.Api.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadingIsGoodService.Api.Extensions;

namespace ReadingIsGoodService.Api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class OrdersController : BaseController
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpGet]
        [Route("orders")]
        [Route("customers/{customerId}/orders")]
        public async Task<BaseApiDataModel<List<OrderDetailModel>>> GetCustomerOrders(int customerId)
        {
            return await Execute(async () =>
            {
                return (await _orderService.GetCustomerOrders(customerId)).Select(c => c.Map()).ToList();
            });
        }

        [HttpGet]
        [Route("orders/{id}")]
        public async Task<BaseApiDataModel<OrderDetailModel>> GetOrder(int id)
        {
            return await Execute(async () =>
            {
                var order = await _orderService.GetOrder(id);
                return order == null ? null : order.Map();
            });
        }

        [HttpPost]
        [Route("orders")]
        public async Task<BaseApiDataModel<OrderDetailModel>> CreateOrder(CreateOrderModel orderModel)
        {
            return await Execute(async () =>
            {
                var order = await _orderService.CreateOrder(orderModel.Map(), userId: HttpContext.GetCurrentUserId());
                return order == null ? null : order.Map();
            });
        }
    }
}
