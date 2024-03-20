namespace OnlineShopProject.Dto.CarDTO
{
    public class CartDto
    {
        public int AppUserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
