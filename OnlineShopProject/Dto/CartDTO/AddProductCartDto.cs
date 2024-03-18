using System.ComponentModel.DataAnnotations;

namespace OnlineShopProject.Dto.CartDTO
{
    public class AddProductCartDto
    {
        public int ProductId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
        public int Quantity { get; set; }
    }
}
