using AutoMapper;
using OnlineShopProject.Dto.ProductDTO;
using OnlineShopProject.Dto.UserDTO;
using OnlineShopProject.Models;

namespace OnlineShopProject.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper() 
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoriesDto, opt => opt.MapFrom(src => src.Categories));
        }
    }
}
