using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopProject.Data;
using OnlineShopProject.Mappers;
using OnlineShopProject.Models;
using OnlineShopProject.Repository;
using System.Runtime.InteropServices;

namespace OnlineShopProject.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var products = string.IsNullOrEmpty(search)
            ? await _productRepository.GetAllAsync()
            : await _productRepository.GetAllBySearchNameAsync(search);

            var productsDto = products.Select(p => p.ToProductDto()).ToList();
            return Ok(productsDto);
        }

        [HttpGet("category")]
        public async Task<IActionResult> GetAllProductsByCategories([FromQuery] string categoryName)
        {
            List<string> categoriesList = categoryName.Split('%').ToList();
            var categories = await _categoryRepository.GetByNamesAsync(categoriesList);
            var products = await _productRepository.GetAllByCategoryNamesAsync(categories);

            var productsDto = products.Select(p => p.ToProductDto()).ToList();
            return Ok(productsDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product.ToProductDto());
        }
    }
}
