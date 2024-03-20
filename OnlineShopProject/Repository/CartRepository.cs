using Microsoft.EntityFrameworkCore;
using OnlineShopProject.Data;
using OnlineShopProject.Enums;
using OnlineShopProject.Models;

namespace OnlineShopProject.Repository
{
    public interface ICartRepository
    {
        public Task<Cart> AddToCartAsync(Cart cart);
        public Task<List<Cart>> GetAllProductsFromCartAsync(AppUser user);
        public Task<Cart> UpdateSelectionStatusAsync(Cart cart);
        public Task<List<Cart>> GetSelectedProductsFromCartAsync(AppUser user);
        public Task<List<Cart>> RemoveProductsFromCart(List<int> productIds, AppUser user);
        public Task<Cart> DeleteProductFromCart(int productId, AppUser user);
    }

    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDBContext _context;
        public CartRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Cart> AddToCartAsync(Cart cart)
        {
            var existingEntry = await _context.Cart.FirstOrDefaultAsync(
                pc => pc.ProductId == cart.ProductId && pc.AppUserId == cart.AppUserId);
            if (existingEntry != null)
            {
                existingEntry.Quantity = cart.Quantity;
            }
            else
            {
                await _context.AddAsync(cart);
            }
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> DeleteProductFromCart(int productId, AppUser user)
        {
            var cart = await _context.Cart.FirstOrDefaultAsync(c => c.ProductId == productId & c.AppUserId == user.Id);
            if (cart != null)
            {
                var deletedCart = _context.Remove(cart).Entity;
                await _context.SaveChangesAsync();
                return deletedCart;
            }
            return null;
        }

        public async Task<List<Cart>> GetAllProductsFromCartAsync(AppUser user)
        {
            return await _context.Cart.Where(pc => pc.AppUserId == user.Id).Include(pc => pc.Product).ToListAsync();
        }

        public async Task<List<Cart>> GetSelectedProductsFromCartAsync(AppUser user)
        {
            return await _context.Cart.Where(pc => pc.AppUserId == user.Id).Include(pc => pc.Product)
                .Where(pc => pc.Status == SelectionStatus.Selected).ToListAsync();
        }

        public async Task<List<Cart>> RemoveProductsFromCart(List<int> productIds, AppUser user)
        {
            var cartItemsToRemove = await _context.Cart.Where(cart => productIds
                .Contains(cart.ProductId) & cart.AppUserId == user.Id).ToListAsync();
            _context.Cart.RemoveRange(cartItemsToRemove);
            await _context.SaveChangesAsync();
            return cartItemsToRemove;
        }

        public async Task<Cart> UpdateSelectionStatusAsync(Cart cart)
        {
            var existingEntry = await _context.Cart.FirstOrDefaultAsync(
                pc => pc.ProductId == cart.ProductId && pc.AppUserId == cart.AppUserId);
            if (existingEntry != null)
            {
                existingEntry.Status = cart.Status;
                await _context.SaveChangesAsync();
            }
            return existingEntry;
        }
    }
}
