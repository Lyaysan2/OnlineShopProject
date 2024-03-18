namespace OnlineShopProject.Dto.UserDTO
{
    public class UserTokenDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
    }
}
