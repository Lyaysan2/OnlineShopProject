using System.ComponentModel.DataAnnotations;

namespace OnlineShopProject.Dto.CategoryDTO
{
    public class AddCategoryDto
    {
        [Required]
        public string Name { get; set; }
    }
}
