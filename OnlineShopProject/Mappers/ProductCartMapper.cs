using OnlineShopProject.Dto.CarDTO;
using OnlineShopProject.Dto.CartDTO;
using OnlineShopProject.Models;

namespace OnlineShopProject.Mappers
{
    public static class ProductCartMapper
    {
        public static Cart ToCartModel(this AddProductCartDto dto, AppUser user)
        { 
            return new Cart
            {
                ProductId = dto.ProductId,
                AppUserId = user.Id,
                Quantity = dto.Quantity,
            };
        }

        public static CartSavedDto ToCartSavedDto(this Cart cart)
        {
            return new CartSavedDto
            {
                ProductId = cart.ProductId,
                AppUserId = cart.AppUserId,
                Quantity = cart.Quantity,
                Status = cart.Status.ToString(),
                CreatedAt = cart.CreatedAt
            };
        }

        public static CartExtendedDto ToCartExtendedDto(this Cart cart)
        {
            return new CartExtendedDto
            {
                ProductId = cart.ProductId,
                Name = cart.Product.Name,
                Description = cart.Product.Description,
                Price = cart.Product.Price,
                Quantity = cart.Quantity,
                Status = cart.Status.ToString(),
                CreatedAt = cart.CreatedAt
            };
        }

        //public static UserProductDto ToUserProductDto(this OrderProduct orderProduct)
        //{
        //    return new UserProductDto
        //    {
        //        ProductId = orderProduct.ProductId,
        //        Name = orderProduct.Product.Name,
        //        Description = orderProduct.Product.Description,
        //        Price = orderProduct.Product.Price,
        //        Quantity = orderProduct.Quantity,
        //        CreatedAt = orderProduct.CreatedAt
        //    };
        //}

        public static Cart ToProductCartSelectionChangedEntity(this ProductSelectionChangeDto dto, AppUser user)
        {
            return new Cart
            {
                ProductId = dto.ProductId,
                AppUserId = user.Id,
                Status = dto.SelectionStatus
            };
        }
    }
}
