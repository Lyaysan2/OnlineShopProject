using AutoMapper;
using Newtonsoft.Json.Linq;
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
        }

        //public static AppUser ToUserEntity(this SignUpDto signUp)
        //{
        //    return new AppUser
        //    {
        //        UserName = signUp.Username,
        //        Email = signUp.Email
        //    };
        //}

        //public static UserDto ToUserCreatedDto(this AppUser user)
        //{
        //    return new UserDto
        //    {
        //        Id = user.Id,
        //        Username = user.UserName,
        //        Email = user.Email
        //    };
        //}

        //public static UserTokenDto ToUserTokenDto(this AppUser user, string token, DateTime expires)
        //{
        //    return new UserTokenDto
        //    {
        //        UserName = user.UserName,
        //        Email = user.Email,
        //        Token = token,
        //        Expires = expires
        //    };
        //}
    }
}
