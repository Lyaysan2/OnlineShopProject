using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShopProject.Dto.CarDTO;
using OnlineShopProject.Dto.CartDTO;
using OnlineShopProject.Mappers;
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

        public CartController(UserManager<AppUser> userManager, ICartRepository cartRepository,
            IProductRepository productRepository, IMapper mapper)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductCartDto addProductCartDto)
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
            var savedProductCart = await _cartRepository.CreateAsync(cart);
            var savedProductCartDto = _mapper.Map<CartSavedDto>(savedProductCart);
            return Ok(savedProductCartDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsInCart()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);

            var products = await _cartRepository.GetAllProductsFromCartAsync(user);
            var productsDto = _mapper.Map<List<CartExtendedDto>>(products);
            return Ok(productsDto);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeProductSelection([FromBody] ProductSelectionChangeDto productSelectionDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);

            var productCart = _mapper.Map<Cart>((productSelectionDto, user));
            var updatedProductCart = await _cartRepository.UpdateSelectionStatucAsync(productCart);
            if (updatedProductCart == null) return BadRequest("Cart not found");

            var updatedProductCartDto = _mapper.Map<CartSavedDto>(updatedProductCart);
            return Ok(updatedProductCartDto);
        }
    }
}
