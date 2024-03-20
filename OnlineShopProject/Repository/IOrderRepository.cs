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
}
