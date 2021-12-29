using ReadingIsGoodService.Common.Models;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Data.Repositories
{
    public interface IProductRepository
    {
        Task<ProductModel> GetProduct(int id);
        Task StockQuantityIncrement(int productId, int value, int userId);
        Task StockQuantityDecrement(int productId, int value, int userId);
    }
}