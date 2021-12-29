using Microsoft.EntityFrameworkCore;
using ReadingIsGoodService.Common.Enums;
using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Data.Repositories
{
    internal class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
    {
        private readonly ReadingIsGoodDbContext _dbContext;

        public override EntityType EntityType => EntityType.Product;

        public ProductRepository(ReadingIsGoodDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override void AddEntity(ProductEntity entity)
        {
            _dbContext.Products.Add(entity);
        }

        public async Task<ProductModel> GetProduct(int id)
        {
            var product = await _dbContext.Products.Where(r => r.Id == id).FirstOrDefaultAsync();

            //todo: move mapping to mapper
            return product == null ? null : new ProductModel { Name = product.Name, Price = product.Price, StockQuantity = product.StockQuantity };
        }

        public async Task StockQuantityIncrement(int productId, int value, int userId)
        {
            var entity = await _dbContext.Products.Where(r => r.Id == productId).FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new Exception($"Product with id {productId} not found");
            }
            if (entity.StockQuantity + value < 0)
            {
                throw new Exception($"Stock quantity of product with id {productId} is not enogh for the operation.");
            }

            entity.UpdatedDate = DateTime.UtcNow;
            entity.StockQuantity += value;

            await Update(entity, userId);
        }

        public async Task StockQuantityDecrement(int productId, int value, int userId)
        {
            await StockQuantityIncrement(productId, -1 * value, userId);
        }
    }
}