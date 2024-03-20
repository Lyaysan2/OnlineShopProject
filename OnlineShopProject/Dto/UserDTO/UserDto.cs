using AutoMapper;
using OnlineShopProject.Models;

namespace OnlineShopProject.Dto.UserDTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
