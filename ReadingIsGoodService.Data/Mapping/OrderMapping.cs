using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Data.Entities;
using System.Linq;

namespace ReadingIsGoodService.Data.Mapping
{
    public static class OrderMapping
    {
        public static OrderModel Map(this OrderEntity order)
        {
            return new OrderModel
            {
                Customer = new CustomerModel
                {
                    Id = order.Customer.Id,
                    Name = order.Customer.Name,
                    Email = order.Customer.Email
                },
                Status = order.Status,
                Items = order.Items.Select(i => i.Map()).ToList(),
                CreatedDate = order.CreatedDate
            };
        }
    }
}
