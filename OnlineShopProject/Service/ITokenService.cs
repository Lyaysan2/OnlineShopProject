using OnlineShopProject.Models;

namespace OnlineShopProject.Service
{
    public interface ITokenService
    {
        Task<(string token, DateTime expires)> CreateToken(AppUser user);
    }
}
