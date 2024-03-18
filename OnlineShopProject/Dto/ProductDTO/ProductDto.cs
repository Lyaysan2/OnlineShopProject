using OnlineShopProject.Dto.CategoryDTO;

namespace OnlineShopProject.Dto.ProductDTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Price { get; set; }
        public List<CategoryDto> CategoriesDto { get; set; }
    }
}
