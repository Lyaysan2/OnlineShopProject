using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShopProject.Dto.OrderDTO;
using OnlineShopProject.Mappers;
using OnlineShopProject.Models;
using OnlineShopProject.Repository;
using System.Security.Claims;

namespace OnlineShopProject.Controllers
{
    [Route("api/order")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderProductRepository _orderProductRepository;
        private readonly IMapper _mapper;
        public OrderController (UserManager<AppUser> userManager, ICartRepository cartRepository, IProductRepository productRepository, 
            IOrderRepository orderRepository, IOrderProductRepository orderProductRepository, IMapper mapper)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);

            var orders = await _orderRepository.GetAllOrdersAsync(user);
            var ordersDto = _mapper.Map<ICollection<OrderDto>>(orders);
            return Ok(ordersDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById([FromRoute] int id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);

            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return NotFound("Order not found");
            if (order.AppUserId != user.Id) return Forbid();

            var orderProducts = await _orderProductRepository.GetOrderProductsByOrderIdAsync(order.Id);
            var orderProductsDto = _mapper.Map<OrderProductExtendedDto>((orderProducts, order));
            return Ok(orderProductsDto);

        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);

            //check if there is unpaid order
            var orderNotCompleted = await _orderRepository.GetOrderInProcessStatusByUserAsync(user);
            if (orderNotCompleted != null) return Conflict("The order has not yet been completed");

            //include products
            var productsCart = await _cartRepository.GetSelectedProductsFromCartAsync(user);

            //get product quantities from stock
            var quantitiesProduct = await _productRepository.GetQuantityListProductsAsync(productsCart.Select(p => p.ProductId).ToList());

            //get reserved product quantities
            var quantitiesOrderProduct = await _orderProductRepository.GetQuantityListOrderProductsReservedAsync(productsCart.Select(p => p.ProductId).ToList());

            //checking for all products in the order
            var quantitiesResult = quantitiesProduct.Select((x, i) => x - quantitiesOrderProduct[i] - productsCart[i].Quantity).ToList();

            var negResult = new Dictionary<string, int>();
            foreach (var (value, i) in quantitiesResult.Select((value, i) => (value, i)))
            {
                //if the user took more products than possible
                if (value < 0)
                {
                    negResult.Add(productsCart[i].Product.Name, -value);
                }
            }
            if (negResult.Count > 0)
            {
                //displays products that cannot be ordered, because they need to be reduced by such quantity
                return BadRequest(negResult);
            }

            var order = await _orderRepository.CreateOrderAsync(_mapper.Map<Order>((productsCart, user)));
            var orderProducts = productsCart.Select(x => _mapper.Map<(Cart, Order), OrderProduct>((x, order))).ToList();
            var savedOrderProducts = await _orderProductRepository.CreateOrderProductAsync(orderProducts);
            var orderProductsDto = _mapper.Map<OrderProductExtendedDto>((orderProducts, order));
            return Ok(orderProductsDto);
        }
    }
}

