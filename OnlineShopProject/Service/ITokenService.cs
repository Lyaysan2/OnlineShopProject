using OnlineShopProject.Models;

namespace OnlineShopProject.Service
{
    public interface ITokenService
    {
        (string token, DateTime expires) CreateToken(AppUser user);
    }
}
