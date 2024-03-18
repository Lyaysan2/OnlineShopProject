using OnlineShopProject.Enums;

namespace OnlineShopProject.Dto.CarDTO
{
    public class CartSavedDto
    {
        public int AppUserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
