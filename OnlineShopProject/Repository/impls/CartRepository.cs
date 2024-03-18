using Microsoft.EntityFrameworkCore;
using OnlineShopProject.Data;
using OnlineShopProject.Enums;
using OnlineShopProject.Models;

namespace OnlineShopProject.Repository.impls
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDBContext _context;
        public CartRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Cart> CreateAsync(Cart cart)
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

        public async Task<List<Cart>> GetAllProductsFromCartAsync(AppUser user)
        {
            return await _context.Cart.Where(pc => pc.AppUserId == user.Id).Include(pc => pc.Product).ToListAsync();
        }

        public async Task<List<Cart>> GetSelectedProductsFromCartAsync(AppUser user)
        {
            return await _context.Cart.Where(pc => pc.AppUserId == user.Id).Include(pc => pc.Product).Where(pc => pc.Status == SelectionStatus.Selected).ToListAsync();
        }

        public async Task<List<Cart>> RemoveProductsFromCart(List<int> productIds, AppUser user)
        {
            var cartItemsToRemove = await _context.Cart.Where(cart => productIds.Contains(cart.ProductId) & cart.AppUserId == user.Id).ToListAsync();
            _context.Cart.RemoveRange(cartItemsToRemove);
            await _context.SaveChangesAsync();
            return cartItemsToRemove;
        }

        public async Task<Cart> UpdateSelectionStatucAsync(Cart cart)
        {
            var existingEntry = await _context.Cart.FirstOrDefaultAsync(
                pc => pc.ProductId == cart.ProductId && pc.AppUserId == cart.AppUserId);
            if (existingEntry != null)
            {
                existingEntry.Status = cart.Status;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("нет такого продукта в корзине");
            }
            return existingEntry;
        }
    }
}
