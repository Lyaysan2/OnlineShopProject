using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShopProject.Mappers;
using OnlineShopProject.Models;
using OnlineShopProject.Repository;
using System.Security.Claims;

namespace OnlineShopProject.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderProductRepository _orderProductRepository;
        public OrderController (UserManager<AppUser> userManager, ICartRepository cartRepository, IProductRepository productRepository, 
            IOrderRepository orderRepository, IOrderProductRepository orderProductRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllOrders()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);
            var orders = await _orderRepository.GetAllOrdersAsync(user);
            var ordersDto = orders.Select(o => o.ToOrderDto()).ToList();
            return Ok(ordersDto);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById([FromRoute] int id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order.AppUserId != user.Id)
            {
                return NotFound("неправильный юзер");
            }
            var orderProducts = await _orderProductRepository.GetOrderProductsByOrderIdAsync(order.Id);
            return Ok(orderProducts.ToOrderProductExtendedDto(order));

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);
            //include products
            var productsCart = await _cartRepository.GetSelectedProductsFromCartAsync(user);

            var quantitiesProduct = await _productRepository.GetQuantityListProductsAsync(productsCart.Select(p => p.ProductId).ToList());
            var quantitiesOrderProduct = await _orderProductRepository.GetQuantityListOrderProductsAsync(productsCart.Select(p => p.ProductId).ToList());
            var quantitiesResult = quantitiesProduct.Select((x, i) => x - quantitiesOrderProduct[i] - productsCart[i].Quantity).ToList();

            var negResult = new Dictionary<string, int>();
            foreach (var (value, i) in quantitiesResult.Select((value, i) => (value, i)))
            {
                if (value < 0)
                {
                    negResult.Add(productsCart[i].Product.Name, -value);
                }
            }
            if (negResult.Count > 0)
            {
                // передает продукты, которые нельзя заказать, тк нужно их нужно уменьшить на такое-то количество
                return BadRequest(negResult);
            }

            var order = await _orderRepository.CreateOrderAsync(productsCart.ToOrderEntity(user));
            var orderProducts = productsCart.Select(x => x.ToOrderProductEntity(order)).ToList();
            var savedOrderProducts = await _orderProductRepository.CreateOrderProductAsync(orderProducts);
            return Ok(orderProducts.ToOrderProductExtendedDto(order));
        }
    }
}

