using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShopProject.Enums;
using OnlineShopProject.Mappers;
using OnlineShopProject.Models;
using OnlineShopProject.Repository;
using System.Security.Claims;

namespace OnlineShopProject.Controllers
{
    [Route("api/pay")]
    [ApiController]
    public class PayController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderProductRepository _orderProductRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        public PayController(UserManager<AppUser> userManager, IOrderRepository orderRepository, IOrderProductRepository orderProductRepository, ICartRepository cartRepository, IProductRepository productRepository) 
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PayOrder(bool IsSuccessful)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);
            var order = await _orderRepository.GetOrderByUserAsync(user);
            if (IsSuccessful && DateTime.Compare(DateTime.UtcNow, order.CreatedAt.AddMinutes(10)) < 0)
            {
                var orderProducts = await _orderProductRepository.ChangeReservProductAsync(order.Id, ReservationStatus.NotReserved);
                order = await _orderRepository.ChangeOrderStatusAsync(order, OrderStatus.Delivering);
                await _cartRepository.RemoveProductsFromCart(orderProducts.Select(op => op.ProductId).ToList(), user);
                await _productRepository.ReduceProductQuantity(orderProducts.ToDictionary(op => op.ProductId, op => op.Quantity));
                var orderProductDto = orderProducts.ToOrderProductExtendedDto(order);
                return Ok(orderProductDto);
            }
            else
            {
                var orderProducts = await _orderProductRepository.ChangeReservProductAsync(order.Id, ReservationStatus.NotReserved);
                order = await _orderRepository.ChangeOrderStatusAsync(order, OrderStatus.NotSuccessful);
                return Forbid("Operation not successful");
            }
        }
    }
}
