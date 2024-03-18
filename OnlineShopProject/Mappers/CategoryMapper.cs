using OnlineShopProject.Dto.CategoryDTO;
using OnlineShopProject.Models;

namespace OnlineShopProject.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToCategoryDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static Category ToCategoryEntity(this AddCategoryDto addCategoryDto)
        {
            return new Category
            {
                Name = addCategoryDto.Name
            };
        }
    }
}
