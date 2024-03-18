using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopProject.Dto.CategoryDTO;
using OnlineShopProject.Mappers;
using OnlineShopProject.Repository;

namespace OnlineShopProject.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository) 
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var category = await _categoryRepository.GetAllAsync();
            var categoryDto = category.Select(c => c.ToCategoryDto()).ToList();
            return Ok(categoryDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoryDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var category = categoryDto.ToCategoryEntity();
            var createdCategory = await _categoryRepository.CreateCategoryAsync(category);
            return Ok(createdCategory.ToCategoryDto());
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] AddCategoryDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var category = categoryDto.ToCategoryEntity();
            var updatedCategory = await _categoryRepository.UpdateCategoryAsync(id, category);
            if (updatedCategory == null)
            {
                return NotFound();
            }
            return Ok(updatedCategory.ToCategoryDto());
        }
    }
}
