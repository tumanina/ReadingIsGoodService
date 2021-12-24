using ReadingIsGoodService.Common.Enums;
using System.Collections.Generic;

namespace ReadingIsGoodService.Data.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public ProductType Type { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public virtual ICollection<OrderItemEntity> Items { get; set; } = new List<OrderItemEntity>();
    }
}