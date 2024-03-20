using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShopProject.Dto.CarDTO;
using OnlineShopProject.Dto.CartDTO;
using OnlineShopProject.Models;
using OnlineShopProject.Repository;
using System.Security.Claims;

namespace OnlineShopProject.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CartController> _logger;

        public CartController(UserManager<AppUser> userManager, ICartRepository cartRepository,
            IProductRepository productRepository, IMapper mapper, ILogger<CartController> logger)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateProductToCart([FromBody] AddProductCartDto addProductCartDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var product = await _productRepository.GetByIdAsync(addProductCartDto.ProductId);
            if (product == null)
            {
                return BadRequest("Product not found");
            }
            if (product.Quantity < addProductCartDto.Quantity)
            {
                return BadRequest("There is no such quantity of product in stock");
            }

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);

            var cart = _mapper.Map<Cart>((addProductCartDto, user));
            var savedProductCart = await _cartRepository.AddToCartAsync(cart);
            var savedProductCartDto = _mapper.Map<CartDto>(savedProductCart);
            _logger.LogInformation($"User {savedProductCart.AppUserId} added a product - {savedProductCart.ProductId}," +
                $" with quantity - {savedProductCart.Quantity} to the cart");
            return Ok(savedProductCartDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProductFromCart([FromRoute] int id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);

            var cart = await _cartRepository.DeleteProductFromCart(id, user);
            if (cart == null) return BadRequest("Product not found");
            var deletedCartDto = _mapper.Map<CartDto>(cart);
            return Ok(deletedCartDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsInCart()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);

            var products = await _cartRepository.GetAllProductsFromCartAsync(user);
            var productsDto = _mapper.Map<List<CartExtendedDto>>(products);

            _logger.LogInformation($"User {user.Id} called all products from cart");
            return Ok(productsDto);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeProductSelection([FromBody] ProductSelectionChangeDto productSelectionDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);

            var productCart = _mapper.Map<Cart>((productSelectionDto, user));
            var updatedProductCart = await _cartRepository.UpdateSelectionStatusAsync(productCart);
            if (updatedProductCart == null) return BadRequest("Cart not found");

            var updatedProductCartDto = _mapper.Map<CartDto>(updatedProductCart);

            _logger.LogInformation($"User {user.Id} changed product - {productSelectionDto.ProductId} " +
                $"selection to {productSelectionDto.SelectionStatus}");
            return Ok(updatedProductCartDto);
        }
    }
}
