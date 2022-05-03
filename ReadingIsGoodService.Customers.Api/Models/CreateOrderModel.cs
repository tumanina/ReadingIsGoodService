using ReadingIsGoodService.Common.Models;
using System.Collections.Generic;

namespace ReadingIsGoodService.CustomersApi.Models
{
    public class CreateOrderModel
    {
        public int CustomerId { get; set; }
        public IEnumerable<OrderItemModel> OrderItems { get; set; }
    }
}
