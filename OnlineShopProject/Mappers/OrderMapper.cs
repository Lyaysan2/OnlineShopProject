using AutoMapper;
using OnlineShopProject.Dto.OrderDTO;
using OnlineShopProject.Models;

namespace OnlineShopProject.Mappers
{
    public class OrderMapper : Profile
    {
        public OrderMapper() 
        {
            CreateMap<(List<Cart>, AppUser), Order>()
                .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.Item2.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => Enums.OrderStatus.InProcess))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Item1.Sum(c => c.Product.Price * c.Quantity)));

            CreateMap<(Cart, Order), OrderProduct>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Item1.ProductId))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Item2.Id))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Item1.Quantity));

            CreateMap<OrderProduct, OrderProductDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));

            CreateMap<(List<OrderProduct>, Order), OrderProductExtendedDto>()
                 .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Item2.Id))
                 .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Item2.Status.ToString()))
                 .ForMember(dest => dest.OrderCreatedAt, opt => opt.MapFrom(src => src.Item2.CreatedAt))
                 .ForMember(dest => dest.OrderCompletedAt, opt => opt.MapFrom(src => src.Item2.CompletedAt))
                 .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Item1.Sum(op => op.Product.Price * op.Quantity)))
                 .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Item1));

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
