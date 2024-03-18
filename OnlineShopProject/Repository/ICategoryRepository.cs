using OnlineShopProject.Models;

namespace OnlineShopProject.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<List<Category>> GetByNamesAsync(List<string> names);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(int id, Category category);
    }
}
