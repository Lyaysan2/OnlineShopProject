namespace OnlineShopProject.Dto.CartDTO
{
    public class CartExtendedDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
