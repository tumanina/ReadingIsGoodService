﻿namespace ReadingIsGoodService.Api.Models
{
    public class CreateCustomerModel
    {
        public string Name { get; set; }
        //todo: create model for address and one-to-many relation
        public string Address { get; set; }
    }
}
