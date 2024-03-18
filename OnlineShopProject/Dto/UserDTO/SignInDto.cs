using System.ComponentModel.DataAnnotations;

namespace OnlineShopProject.Dto.UserDTO
{
    public class SignInDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
