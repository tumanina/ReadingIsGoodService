using ReadingIsGoodService.Common.Enums;
using ReadingIsGoodService.OrdersApi.Models;
using System;
using System.Collections.Generic;

namespace ReadingIsGoodService.OrdersApi.Models
{
    public class OrderDetailModel
    {
        public int Id { get; set; }
        public CustomerDetailModel Customer { get; set; }
        public IEnumerable<BaseOrderItemModel> ProductItems { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
