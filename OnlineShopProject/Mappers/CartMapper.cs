using AutoMapper;
using OnlineShopProject.Dto.CarDTO;
using OnlineShopProject.Dto.CartDTO;
using OnlineShopProject.Models;

namespace OnlineShopProject.Mappers
{
    public class CartMapper : Profile
    {
        public CartMapper() 
        {
            CreateMap<(AddProductCartDto, AppUser), Cart>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Item1.ProductId))
                .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.Item2.Id))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Item1.Quantity));

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Cart, CartExtendedDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Product.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<(ProductSelectionChangeDto, AppUser), Cart>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Item1.ProductId))
                .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.Item2.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Item1.SelectionStatus.ToString()));
        }
    }
}
