using ReadingIsGoodService.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Logic.Interfaces
{
    public interface IOrderService
    {
        Task<OrderModel> GetOrder(int id);
        Task<IEnumerable<OrderModel>> GetCustomerOrders(int customerId);
        Task<OrderModel> CreateOrder(OrderModel order);
    }
}
