using ReadingIsGoodService.Common.Enums;
using ReadingIsGoodService.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Data.Repositories
{
    internal interface IOrderRepository
    {
        Task<OrderModel> GetOrder(int id);
        Task<IEnumerable<OrderModel>> GetCustomerOrders(int customerId);
        Task<int> CreateOrder(int customerId, IEnumerable<OrderItemModel> items);
        Task UpdateStatus(int orderId, OrderStatus status);
    }
}
