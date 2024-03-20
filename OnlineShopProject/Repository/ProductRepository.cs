using Microsoft.EntityFrameworkCore;
using OnlineShopProject.Data;
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

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductRepository(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Product.Include(p => p.Categories).ToListAsync();
        }

        public async Task<List<Product>> GetAllByCategoryNamesAsync(List<Category> categories)
        {
            var catIds = categories.Select(c => c.Id).ToList();
            return await _context.Product.Where(p => catIds.All(c => p.Categories.Select(i => i.Id).Contains(c)))
                .Include(p => p.Categories).ToListAsync();
        }

        public async Task<List<Product>> GetAllBySearchNameAsync(string searchName)
        {
            return await _context.Product.Where(p => p.Name.Contains(searchName)).Include(p => p.Categories).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Product
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<int>> GetQuantityListProductsAsync(List<int> productIds)
        {
            var quantities = await _context.Product
                .Where(pc => productIds.Contains(pc.Id))
                .Select(pc => new { pc.Id, pc.Quantity })
                .ToListAsync();

            var quantityList = productIds.Select(productId =>
                quantities.FirstOrDefault(q => q.Id == productId)?.Quantity ?? 0).ToList();

            return quantityList;
        }

        public async Task<List<Product>> ReduceProductQuantity(Dictionary<int, int> productQuantity)
        {
            List<Product> updatedProducts = new List<Product>();

            foreach (var entry in productQuantity)
            {
                var productId = entry.Key;
                var quantityToReduce = entry.Value;

                var product = await _context.Product.FindAsync(productId);

                if (product != null)
                {
                    product.Quantity -= quantityToReduce;
                    updatedProducts.Add(product);
                }
            }
            await _context.SaveChangesAsync();

            return updatedProducts;
        }
    }
}
