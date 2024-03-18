using OnlineShopProject.Models;

namespace OnlineShopProject.Repository
{
    public interface ICartRepository
    {
        public Task<Cart> CreateAsync(Cart cart);
        public Task<List<Cart>> GetAllProductsFromCartAsync(AppUser user);
        public Task<Cart> UpdateSelectionStatucAsync(Cart cart);
        public Task<List<Cart>> GetSelectedProductsFromCartAsync(AppUser user);
        public Task<List<Cart>> RemoveProductsFromCart(List<int> productIds, AppUser user);
    }
}
