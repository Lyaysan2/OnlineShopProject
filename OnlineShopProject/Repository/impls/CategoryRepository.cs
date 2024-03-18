using Microsoft.EntityFrameworkCore;
using OnlineShopProject.Data;
using OnlineShopProject.Models;

namespace OnlineShopProject.Repository.impls
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _context;
        public CategoryRepository(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<List<Category>> GetByNamesAsync(List<string> names)
        {
            var allCategories = await GetAllAsync();
            var categories = names.Select(name => allCategories.FirstOrDefault(c => c.Name == name));
            if (categories.Contains(null))
            {
                //todo
                throw new Exception("Неверные имена категорий в запросе");
            }
            return categories.ToList();
        }

        public async Task<Category> UpdateCategoryAsync(int id, Category category)
        {
            var existingCategory = await _context.Category.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                await _context.SaveChangesAsync();
            }
            return existingCategory;
        }
    }
}
