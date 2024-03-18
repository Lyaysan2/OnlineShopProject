using Microsoft.AspNetCore.Identity;

namespace OnlineShopProject.Models
{
    public class AppUser : IdentityUser<int>
    { 
        public List<Order> Orders { get; set; } = new();
    }   
}
