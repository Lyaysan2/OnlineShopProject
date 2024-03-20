using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopProject.Dto.CategoryDTO;
using OnlineShopProject.Models;
using OnlineShopProject.Repository;

namespace OnlineShopProject.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoryController> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var category = await _categoryRepository.GetAllAsync();
            var categoryDto = _mapper.Map<ICollection<CategoryDto>>(category);
            _logger.LogInformation("Get all category called");
            return Ok(categoryDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoryDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var category = _mapper.Map<Category>(categoryDto);
            var createdCategory = await _categoryRepository.CreateCategoryAsync(category);
            var createdCategoryDto = _mapper.Map<CategoryDto>(createdCategory);
            _logger.LogInformation($"Category - {createdCategory.Id} created");
            return Ok(createdCategoryDto);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] AddCategoryDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var category = _mapper.Map<Category>(categoryDto);
            var updatedCategory = await _categoryRepository.UpdateCategoryAsync(id, category);
            if (updatedCategory == null)
            {
                return NotFound();
            }
            var updatedCategoryDto = _mapper.Map<CategoryDto>(updatedCategory);
            _logger.LogInformation($"In сategory - {updatedCategory.Id} name changed to {updatedCategory.Name}");
            return Ok(updatedCategoryDto);
        }
    }
}
