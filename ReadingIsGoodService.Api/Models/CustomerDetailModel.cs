﻿namespace ReadingIsGoodService.Api.Models
{
    public class CustomerDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //todo: create model for address and one-to-many relation
        public string Address { get; set; }
    }
}