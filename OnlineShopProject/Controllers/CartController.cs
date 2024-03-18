using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShopProject.Dto.CartDTO;
using OnlineShopProject.Mappers;
using OnlineShopProject.Models;
using OnlineShopProject.Repository;
using System.Security.Claims;

namespace OnlineShopProject.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartController(UserManager<AppUser> userManager, ICartRepository cartRepository,
            IProductRepository productRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductCartDto addProductCartDto)
        {
            var product = await _productRepository.GetByIdAsync(addProductCartDto.ProductId);
            if (product == null)
            {
                return BadRequest("Такого продукта нет");
            }
            if (product.Quantity < addProductCartDto.Quantity)
            {
                return BadRequest("Нет такого кол-ва продукта на складе");
            }
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);
            var cart = addProductCartDto.ToCartModel(user);
            var savedProductCart = await _cartRepository.CreateAsync(cart);
            return Ok(savedProductCart.ToCartSavedDto());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProductsInCart()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);
            var products = await _cartRepository.GetAllProductsFromCartAsync(user);
            var productsDto = products.Select(p => p.ToCartExtendedDto()).ToList();
            return Ok(productsDto);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangeProductSelection([FromBody] ProductSelectionChangeDto productSelectionDto)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);
            var productCart = productSelectionDto.ToProductCartSelectionChangedEntity(user);
            var updatedProductCart = await _cartRepository.UpdateSelectionStatucAsync(productCart);
            return Ok(updatedProductCart.ToCartSavedDto());
        }
    }
}
