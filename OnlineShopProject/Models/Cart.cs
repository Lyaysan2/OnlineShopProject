using OnlineShopProject.Enums;

namespace OnlineShopProject.Models
{
    public class Cart
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public int Quantity { get; set; }
        public SelectionStatus Status { get; set; } = SelectionStatus.NotSelected;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
