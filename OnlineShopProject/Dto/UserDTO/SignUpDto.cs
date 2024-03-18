using AutoMapper;
using AutoMapper.Configuration.Annotations;
using OnlineShopProject.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopProject.Dto.UserDTO
{
    //[AutoMap(typeof(AppUser))]
    public class SignUpDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        //[Ignore]
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
