using OnlineShopProject.Models;

namespace OnlineShopProject.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetAllBySearchNameAsync(string searchName);
        Task<List<Product>> GetAllByCategoryNamesAsync(List<Category> categories);
        Task<Product?> GetByIdAsync(int id);
        Task<List<int>> GetQuantityListProductsAsync(List<int> productIds);
        Task<List<Product>> ReduceProductQuantity(Dictionary<int, int> productQuantity);
    }
}
