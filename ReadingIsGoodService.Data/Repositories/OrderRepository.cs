using Microsoft.EntityFrameworkCore;
using ReadingIsGoodService.Common.Enums;
using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Data.Repositories
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly ReadingIsGoodDbContext _dbContext;

        public OrderRepository(ReadingIsGoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<OrderModel>> GetCustomerOrders(int customerId)
        {
            var orders = await _dbContext.Orders.Where(r => r.CustomerId == customerId)
                .Include(r => r.Customer)
                .Include(r => r.Items).ThenInclude(i => i.Product)
                .ToListAsync();

            //todo: move mapping to mapper
            return orders.Select(r => new OrderModel
            {
                Customer = new CustomerModel
                {
                    Id = r.Customer.Id,
                    Name = r.Customer.Name,
                    Email = r.Customer.Email
                },
                Status = r.Status,
                Items = r.Items.Select(i => new OrderItemModel
                {
                    Product = new ProductModel { Name = i.Product.Name, Price = i.Product.Price },
                    Quantity = i.Count
                }).ToList(),
                CreatedDate = r.CreatedDate
            });
        }

        public async Task<OrderModel> GetOrder(int id)
        {
            var order = await _dbContext.Orders.Where(r => r.Id == id)
                .Include(r => r.Customer)
                .Include(r => r.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync();

            //todo: move mapping to mapper
            return order == null ? null : new OrderModel
            {
                Customer = new CustomerModel
                {
                    Id = order.Customer.Id,
                    Name = order.Customer.Name,
                    Email = order.Customer.Email
                },
                Status = order.Status,
                Items = order.Items.Select(i => new OrderItemModel
                {
                    Product = new ProductModel { Name = i.Product.Name, Price = i.Product.Price },
                    Quantity = i.Count
                }).ToList(),
                CreatedDate = order.CreatedDate
            };
        }

        public async Task<int> CreateOrder(int customerId, IEnumerable<OrderItemModel> items)
        {
            //todo: move mapping to mapper
            var entity = new OrderEntity
            {
                CustomerId = customerId,
                Status = OrderStatus.Created,
            };

            foreach (var item in items)
            {
                entity.Items.Add(new OrderItemEntity { ProductId = item.ProductId, Count = item.Quantity });
            };

            entity.UpdatedDate = DateTime.UtcNow;
            entity.CreatedDate = DateTime.UtcNow;

            _dbContext.Orders.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateStatus(int orderId, OrderStatus status)
        {
            //todo: validate statuses workflow
            var entity = await GetOrder(orderId);

            if (entity == null)
            {
                //to do: exception
            }

            entity.UpdatedDate = DateTime.UtcNow;
            entity.Status = status;
            _dbContext.Update(entity);

            await _dbContext.SaveChangesAsync();
        }
    }
}