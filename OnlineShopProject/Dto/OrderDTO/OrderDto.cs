using OnlineShopProject.Enums;
using OnlineShopProject.Models;

namespace OnlineShopProject.Dto.OrderDTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public float TotalPrice { get; set; }
    }
}
