using Microsoft.EntityFrameworkCore;
using OnlineShopProject.Data;
using OnlineShopProject.Enums;
using OnlineShopProject.Models;

namespace OnlineShopProject.Repository
{
    public interface IOrderProductRepository
    {
        Task<List<OrderProduct>> CreateOrderProductAsync(List<OrderProduct> orderProducts);
        Task<List<int>> GetQuantityListOrderProductsReservedAsync(List<int> productIds);
        Task<List<OrderProduct>> GetOrderProductsByOrderIdAsync(int orderId);
        Task<List<OrderProduct>> ChangeReservProductAsync(int orderId, ReservationStatus status);
    }

    public class OrderProductRepository : IOrderProductRepository
    {
        private readonly ApplicationDBContext _context;
        public OrderProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<OrderProduct>> CreateOrderProductAsync(List<OrderProduct> orderProducts)
        {
            await _context.AddRangeAsync(orderProducts);
            await _context.SaveChangesAsync();
            foreach (var orderProduct in orderProducts)
            {
                await _context.Entry(orderProduct)
                    .Reference(op => op.Product)
                    .LoadAsync();
            }
            return orderProducts;
        }

        public async Task<List<OrderProduct>> GetOrderProductsByOrderIdAsync(int orderId)
        {
            return await _context.OrderProduct.Include(op => op.Product).Where(op => op.OrderId == orderId).ToListAsync();
        }

        public async Task<List<int>> GetQuantityListOrderProductsReservedAsync(List<int> productIds)
        {
            var quantities = await _context.OrderProduct
                .Where(pc => productIds.Contains(pc.ProductId) & pc.Reserved == ReservationStatus.Reserved)
                .Select(pc => new { pc.ProductId, pc.Quantity })
                .ToListAsync();

            var quantityList = productIds.Select(productId =>
                quantities.FirstOrDefault(q => q.ProductId == productId)?.Quantity ?? 0).ToList();

            return quantityList;
        }

        public async Task<List<OrderProduct>> ChangeReservProductAsync(int orderId, ReservationStatus status)
        {
            var orderProducts = await GetOrderProductsByOrderIdAsync(orderId);
            orderProducts.ForEach(op => op.Reserved = status);
            await _context.SaveChangesAsync();
            return orderProducts;
        }
    }
}
