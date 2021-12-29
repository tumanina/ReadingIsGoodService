using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Data.Entities;
using System.Linq;

namespace ReadingIsGoodService.Data.Mapping
{
    public static class OrderItemMapping
    {
        public static OrderItemModel Map(this OrderItemEntity orderItem)
        {
            return new OrderItemModel
            {
                Product = orderItem.Product.Map(),
                Quantity = orderItem.Count
            };
        }
    }
}
