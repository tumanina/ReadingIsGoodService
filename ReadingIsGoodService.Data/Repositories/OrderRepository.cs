using Microsoft.EntityFrameworkCore;
using ReadingIsGoodService.Common.Enums;
using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Data.Entities;
using ReadingIsGoodService.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Data.Repositories
{
    internal class OrderRepository : BaseRepository<OrderEntity>, IOrderRepository
    {
        private readonly ReadingIsGoodDbContext _dbContext;

        public override EntityType EntityType => EntityType.Order;

        public OrderRepository(ReadingIsGoodDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override void AddEntity(OrderEntity entity)
        {
            _dbContext.Orders.Add(entity);
        }

        public async Task<IEnumerable<OrderModel>> GetCustomerOrders(int customerId)
        {
            var orders = await _dbContext.Orders.Where(r => r.CustomerId == customerId)
                .Include(r => r.Customer)
                .Include(r => r.Items).ThenInclude(i => i.Product)
                .ToListAsync();

            return orders.Select(r => r.Map());
        }

        public async Task<OrderModel> GetOrder(int id)
        {
            var order = await _dbContext.Orders.Where(r => r.Id == id)
                .Include(r => r.Customer)
                .Include(r => r.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync();

            return order == null ? null : order.Map();
        }

        public async Task<int> CreateOrder(int customerId, IEnumerable<OrderItemModel> items, int userId)
        {
            var entity = new OrderEntity
            {
                CustomerId = customerId,
                Status = OrderStatus.Created,
            };

            foreach (var item in items)
            {
                entity.Items.Add(new OrderItemEntity { ProductId = item.ProductId, Count = item.Quantity });
            };

            return await Create(entity, userId);
        }

        public async Task UpdateStatus(int orderId, OrderStatus status, int userId)
        {
            //todo: validate statuses workflow
            var entity = await _dbContext.Orders.Where(r => r.Id == orderId).FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new Exception($"Order with id '{orderId}' not found");
            }

            entity.UpdatedDate = DateTime.UtcNow;
            entity.Status = status;

            await Update(entity, userId);
        }
    }
}
