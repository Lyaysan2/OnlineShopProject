using OnlineShopProject.Dto.OrderDTO;
using OnlineShopProject.Models;

namespace OnlineShopProject.Mappers
{
    public static class OrderMapper
    {
        public static Order ToOrderEntity(this List<Cart> cart, AppUser user)
        {
            return new Order
            {
                AppUserId = user.Id,
                Status = Enums.OrderStatus.InProcess,
                TotalPrice = cart.Sum(c => c.Product.Price * c.Quantity)
            };
        }

        public static OrderProduct ToOrderProductEntity(this Cart cart, Order order)
        {
            return new OrderProduct
            {
                ProductId = cart.ProductId,
                OrderId = order.Id,
                Quantity = cart.Quantity
            };
        }

        public static OrderProductDto ToOrderProductDto(this OrderProduct orderProduct)
        {
            return new OrderProductDto
            {
                ProductId = orderProduct.ProductId,
                ProductName = orderProduct.Product.Name,
                ProductDescription = orderProduct.Product.Description,
                Price = orderProduct.Product.Price,
                Quantity = orderProduct.Quantity,
                CreatedAt = orderProduct.CreatedAt
            };
        }

        public static OrderProductExtendedDto ToOrderProductExtendedDto(this List<OrderProduct> orderProducts, Order order)
        {
            return new OrderProductExtendedDto
            {
                OrderId = order.Id,
                OrderStatus = order.Status.ToString(),
                OrderCreatedAt = order.CreatedAt,
                OrderCompletedAt = order.CompletedAt,
                TotalPrice = orderProducts.Sum(op => op.Product.Price * op.Quantity),
                Products = orderProducts.Select(op => op.ToOrderProductDto()).ToList(),
            };
        }

        public static OrderDto ToOrderDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                CompletedAt = order.CompletedAt,
                TotalPrice = order.TotalPrice
            };
        }
    }
}
