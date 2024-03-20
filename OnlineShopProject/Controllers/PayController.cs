using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShopProject.Dto.OrderDTO;
using OnlineShopProject.Enums;
using OnlineShopProject.Mappers;
using OnlineShopProject.Models;
using OnlineShopProject.Repository;
using System.Security.Claims;

namespace OnlineShopProject.Controllers
{
    [Route("api/pay")]
    [ApiController]
    [Authorize]
    public class PayController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderProductRepository _orderProductRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public PayController(UserManager<AppUser> userManager, IOrderRepository orderRepository, IOrderProductRepository orderProductRepository, 
            ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper) 
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> PayOrder(bool IsSuccessful)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);

            var order = await _orderRepository.GetOrderInProcessStatusByUserAsync(user);
            if (order == null) return NotFound("You have no unpaid orders");

            if (IsSuccessful && DateTime.Compare(DateTime.UtcNow, order.CreatedAt.AddMinutes(10)) < 0)
            {
                //remove reserve from paid products
                var orderProducts = await _orderProductRepository.ChangeReservProductAsync(order.Id, ReservationStatus.NotReserved);

                //change the status of the order to “delivered”
                order = await _orderRepository.ChangeOrderStatusAsync(order, OrderStatus.Delivering);

                //remove paid products from the cart
                await _cartRepository.RemoveProductsFromCart(orderProducts.Select(op => op.ProductId).ToList(), user);

                //reducing the quantity of products in stock
                await _productRepository.ReduceProductQuantity(orderProducts.ToDictionary(op => op.ProductId, op => op.Quantity));

                var orderProductDto = _mapper.Map<OrderProductExtendedDto>((orderProducts, order));
                return Ok(orderProductDto);
            }
            else
            {
                //remove reserve from unpaid products
                var orderProducts = await _orderProductRepository.ChangeReservProductAsync(order.Id, ReservationStatus.NotReserved);

                //change the status of the order to “not successful”
                order = await _orderRepository.ChangeOrderStatusAsync(order, OrderStatus.NotSuccessful);

                return Conflict("Operation not successful");
            }
        }
    }
}
