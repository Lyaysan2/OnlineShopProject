using OnlineShopProject.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.InProcess;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public int? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public float TotalPrice { get; set; }
    }
}
