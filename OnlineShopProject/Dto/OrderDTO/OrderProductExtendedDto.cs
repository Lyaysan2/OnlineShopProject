namespace OnlineShopProject.Dto.OrderDTO
{
    public class OrderProductExtendedDto
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderCreatedAt { get; set; }
        public DateTime? OrderCompletedAt { get; set; }
        public float TotalPrice { get; set; }
        public List<OrderProductDto> Products { get; set; }
    }
}
