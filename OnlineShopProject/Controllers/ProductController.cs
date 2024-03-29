﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineShopProject.Dto.ProductDTO;
using OnlineShopProject.Repository;

namespace OnlineShopProject.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, 
            IMapper mapper, ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var products = string.IsNullOrEmpty(search)
            ? await _productRepository.GetAllAsync()
            : await _productRepository.GetAllBySearchNameAsync(search);

            var productsDto = _mapper.Map<List<ProductDto>>(products);
            _logger.LogInformation("Get all product called");
            return Ok(productsDto);
        }

        [HttpGet("category")]
        public async Task<IActionResult> GetAllProductsByCategories([FromQuery] string categoryName)
        {
            try
            {
                List<string> categoriesList = categoryName.Split('%').ToList();
                var categories = await _categoryRepository.GetByNamesAsync(categoriesList);
                if (categories.Any(c => c == null)) return NotFound("Сategory was not found");

                var products = await _productRepository.GetAllByCategoryNamesAsync(categories);
                var productsDto = _mapper.Map<List<ProductDto>>(products);
                _logger.LogInformation($"Get all product by categories {categories} called");
                return Ok(productsDto);
            } 
            catch
            {
                return BadRequest("Invalid categoryName format");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            var productDto = _mapper.Map<ProductDto>(product);
            _logger.LogInformation($"Get product by id - {id} called");
            return Ok(productDto);
        }
    }
}
