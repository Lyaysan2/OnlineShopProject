using OnlineShopProject.Enums;
using OnlineShopProject.Models;

namespace OnlineShopProject.Repository
{
    public interface IOrderProductRepository
    {
        Task<List<OrderProduct>> CreateOrderProductAsync(List<OrderProduct> orderProducts);
        Task<List<int>> GetQuantityListOrderProductsAsync(List<int> productIds);
        Task<List<OrderProduct>> GetOrderProductsByOrderIdAsync(int orderId);
        Task<List<OrderProduct>> ChangeReservProductAsync(int orderId, ReservationStatus status);
    }
}
