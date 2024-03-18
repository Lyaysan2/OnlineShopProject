using OnlineShopProject.Dto.ProductDTO;
using OnlineShopProject.Models;

namespace OnlineShopProject.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToProductDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoriesDto = product.Categories.Select(c => c.ToCategoryDto()).ToList()
            };
        }
    }
}
