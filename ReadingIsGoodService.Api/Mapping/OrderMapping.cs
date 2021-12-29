using ReadingIsGoodService.Api.Models;
using ReadingIsGoodService.Common.Models;
using System.Linq;
using OrderItemModel = ReadingIsGoodService.Common.Models.OrderItemModel;

namespace ReadingIsGoodService.Api.Mapping
{
    public static class OrderMapping
    {
        public static OrderDetailModel Map(this OrderModel order)
        {
            return new OrderDetailModel
            {
                Id = order.Id,
                Customer = new CustomerDetailModel { Id = order.Customer.Id, Name = order.Customer.Name, Email = order.Customer.Email },
                Status = order.Status,
                ProductItems = order.Items.Select(i => new OrderItemDetailModel { ProductId = i.Product.Id, ProductQuantity = i.Quantity, ProductName = i.Product?.Name }),
                CreatedDate = order.CreatedDate,
                ModifiedDate = order.UpdatedDate
            };
        }
        public static OrderModel Map(this CreateOrderModel order)
        {
            return new OrderModel
            {
                CustomerId = order.CustomerId,
                Items = order.OrderItems?.Select(i => new OrderItemModel { ProductId = i.ProductId, Quantity = i.ProductQuantity })
            };
        }
    }
}
