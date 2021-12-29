using ReadingIsGoodService.Common.Enums;
using System;
using System.Collections.Generic;

namespace ReadingIsGoodService.Tests.IntegrationTests.Models
{
    public class OrderDetailModel
    {
        public IEnumerable<BaseOrderItemModel> ProductItems { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
