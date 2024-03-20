using AutoMapper;
using OnlineShopProject.Dto.UserDTO;
using OnlineShopProject.Models;

namespace OnlineShopProject.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper() 
        {
            CreateMap<SignUpDto, AppUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<AppUser, UserDto>();

            CreateMap<(AppUser, string, DateTime), UserTokenDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Item1.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Item1.Email))
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Item2))
            .ForMember(dest => dest.Expires, opt => opt.MapFrom(src => src.Item3));
        }
    }
}
