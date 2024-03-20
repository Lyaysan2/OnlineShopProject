using OnlineShopProject.Dto.CategoryDTO;
using OnlineShopProject.Dto.ProductDTO;
using OnlineShopProject.Models;
using AutoMapper;

namespace OnlineShopProject.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper() 
        {
            CreateMap<Category, CategoryDto>();

            CreateMap<AddCategoryDto, Category>();
        }
    }
}
