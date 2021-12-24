﻿using System.Collections.Generic;

namespace ReadingIsGoodService.Api.Models
{
    public class CreateOrderModel
    {
        public int CustomerId { get; set; }
        public IEnumerable<BaseOrderItemModel> ProductItems { get; set; }
    }
}