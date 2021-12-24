using ReadingIsGoodService.Common.Enums;
using System.Collections.Generic;

namespace ReadingIsGoodService.Common.Models
{
    public class ProductModel : BaseModel
    {
        public string Name { get; set; }
        public ProductType Type { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }

        public virtual ICollection<OrderItemModel> Items { get; set; }
    }
}