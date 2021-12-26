using ReadingIsGoodService.Common.Enums;
using System;
using System.Collections.Generic;

namespace ReadingIsGoodService.Common.Models
{
    public class OrderModel : BaseModel
    {
        public int CustomerId { get; set; }
        public OrderStatus Status { get; set; }

        public CustomerModel Customer { get; set; }
        public IEnumerable<OrderItemModel> Items { get; set; }
    }
}