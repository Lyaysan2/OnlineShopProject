using AutoMapper;
using AutoMapper.Configuration.Annotations;
using OnlineShopProject.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopProject.Dto.UserDTO
{
    public class SignUpDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
