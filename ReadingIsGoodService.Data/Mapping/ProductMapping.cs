using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Data.Entities;
using System.Linq;

namespace ReadingIsGoodService.Data.Mapping
{
    public static class ProductMapping
    {
        public static ProductModel Map(this ProductEntity product)
        {
            return new ProductModel
            {
                Name = product.Name,
                Price = product.Price
            };
        }
    }
}
