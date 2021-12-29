using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Data.Entities;
using System.Linq;

namespace ReadingIsGoodService.Data.Mapping
{
    public static class CustomerMapping
    {
        public static CustomerModel Map(this CustomerEntity customer)
        {
            return new CustomerModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Orders = customer.Orders.Select(r => new OrderModel
                {
                    Status = r.Status,
                    Items = r.Items.Select(i => i.Map()).ToList(),
                    CreatedDate = r.CreatedDate
                }).ToList()
            };
        }
    }
}
