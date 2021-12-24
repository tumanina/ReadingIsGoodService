using Microsoft.EntityFrameworkCore;
using ReadingIsGoodService.Common.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Data.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly ReadingIsGoodDbContext _dbContext;

        public ProductRepository(ReadingIsGoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductModel> GetProduct(int id)
        {
            var product = await _dbContext.Products.Where(r => r.Id == id).FirstOrDefaultAsync();

            //todo: move mapping to mapper
            return product == null ? null : new ProductModel { Name = product.Name, Price = product.Price };
        }

        public async Task StockQuantityIncrement(int productId, int value)
        {
            var entity = await GetProduct(productId);

            if (entity == null)
            {
                //to do: exception
            }
            if (entity.StockCount + value < 0)
            {
                //to do: exception
            }

            entity.UpdatedDate = DateTime.UtcNow;
            entity.StockCount += value;
            _dbContext.Update(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task StockQuantityDecrement(int productId, int value)
        {
            await StockQuantityDecrement(productId, -1 * value);
        }
    }
}