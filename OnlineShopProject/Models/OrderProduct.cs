using OnlineShopProject.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopProject.Models
{
    public class OrderProduct
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public ReservationStatus? Reserved { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
