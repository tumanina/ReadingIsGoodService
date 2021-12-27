using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadingIsGoodService.Api.Models;
using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Logic.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                //todo: move to mapping
                return (await _orderService.GetCustomerOrders(customerId)).Select(c => new OrderDetailModel
                {
                    Customer = new CustomerDetailModel { Id = c.Customer.Id, Name = c.Customer.Name, Email = c.Customer.Email },
                    Status = c.Status,
                    ProductItems = c.Items.Select(i => new OrderItemDetailModel { ProductQuantity = i.Quantity, ProductName = i.Product?.Name })
                }).ToList();
            });
        }

        [HttpGet]
        [Route("orders/{id}")]
        public async Task<BaseApiDataModel<OrderDetailModel>> GetOrder(int id)
        {
            return await Execute(async () =>
            {
                var order = await _orderService.GetOrder(id);

                if (order == null)
                {
                    return null;
                }

                return new OrderDetailModel
                {
                    Customer = new CustomerDetailModel { Id = order.Customer.Id, Name = order.Customer.Name, Email = order.Customer.Email },
                    Status = order.Status,
                    ProductItems = order.Items.Select(i => new OrderItemDetailModel { ProductQuantity = i.Quantity, ProductName = i.Product?.Name })
                };
            });
        }

        [HttpPost]
        [Route("orders")]
        public async Task<BaseApiDataModel<OrderDetailModel>> CreateOrder(CreateOrderModel orderModel)
        {
            return await Execute(async () =>
            {
                var order = await _orderService.CreateOrder(new OrderModel 
                {
                    CustomerId = orderModel.CustomerId, 
                    Items = orderModel.ProductItems.Select(i => new OrderItemModel { ProductId = i.ProductId, Quantity = i.ProductQuantity })
                });

                if (order == null)
                {
                    return null;
                }

                return new OrderDetailModel
                {
                    Customer = new CustomerDetailModel { Id = order.Customer.Id, Name = order.Customer.Name, Email = order.Customer.Email },
                    Status = order.Status,
                    ProductItems = order.Items.Select(i => new OrderItemDetailModel { ProductQuantity = i.Quantity, ProductName = i.Product?.Name })
                };
            });
        }
    }
}
