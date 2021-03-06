using ReadingIsGoodService.Common.Enums;
using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Data.Repositories;
using ReadingIsGoodService.Orders.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Orders.Logic
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        //todo: add pagination
        public async Task<IEnumerable<OrderModel>> GetCustomerOrders(int customerId)
        {
            return await _orderRepository.GetCustomerOrders(customerId);
        }

        public async Task<OrderModel> GetOrder(int id)
        {
            return await _orderRepository.GetOrder(id);
        }

        public async Task<OrderModel> CreateOrder(OrderModel order, int userId)
        {
            await ValidateOrder(order);
            await ValidateCustomer(order.CustomerId);

            var orderId = await _orderRepository.CreateOrder(order.CustomerId, order.Items, userId);
            if (orderId == 0)
            {
                return null;
            }

            var updatedItems = new List<OrderItemModel>();
            try
            {
                foreach (var item in order.Items)
                {
                    await _productRepository.StockQuantityDecrement(item.ProductId, item.Quantity, userId);
                    updatedItems.Add(item);
                }
                await _orderRepository.UpdateStatus(orderId, OrderStatus.Approved, userId);
            }
            catch (Exception ex)
            {
                //todo: log reason of rejecting
                await _orderRepository.UpdateStatus(orderId, OrderStatus.Rejected, userId);

                //todo: improve to command rollback
                foreach (var item in updatedItems)
                {
                    await _productRepository.StockQuantityIncrement(item.ProductId, item.Quantity, userId);
                }
            }

            return await _orderRepository.GetOrder(orderId);
        }

        private async Task ValidateOrder(OrderModel order)
        {
            if (!order.Items.Any())
            {
                throw new Exception("Order must contain items.");
            }

            foreach (var item in order.Items)
            {
                var product = await _productRepository.GetProduct(item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product with id {item.ProductId} not exist");
                }
                if (product.StockQuantity < item.Quantity)
                {
                    throw new Exception($"Stock quantity of product with id {item.ProductId} less than {item.Quantity}");
                }
            }
        }

        private async Task ValidateCustomer(int customerId)
        {
            //todo: validate that customer exist and active
        }
    }
}
