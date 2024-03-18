namespace OnlineShopProject.Dto.OrderDTO
{
    public class OrderProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
