using Microsoft.EntityFrameworkCore;
using OnlineShopProject.Data;
using OnlineShopProject.Enums;
using OnlineShopProject.Models;

namespace OnlineShopProject.Repository
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<List<Order>> GetAllOrdersAsync(AppUser appUser);
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> ChangeOrderStatusAsync(Order order, OrderStatus status);
        Task<Order> GetOrderInProcessStatusByUserAsync(AppUser user);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDBContext _context;
        public OrderRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Order> ChangeOrderStatusAsync(Order order, OrderStatus status)
        {
            order.Status = status;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetAllOrdersAsync(AppUser appUser)
        {
            return await _context.Order.Where(o => o.AppUserId == appUser.Id).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Order.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> GetOrderInProcessStatusByUserAsync(AppUser user)
        {
            return await _context.Order.FirstOrDefaultAsync(o => o.AppUserId == user.Id & o.Status == OrderStatus.InProcess);
        }
    }
}
