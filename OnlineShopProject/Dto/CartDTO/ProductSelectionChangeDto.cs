using OnlineShopProject.Enums;

namespace OnlineShopProject.Dto.CartDTO
{
    public class ProductSelectionChangeDto
    {
        public int ProductId { get; set; }
        public SelectionStatus SelectionStatus { get; set; }
    }
}
