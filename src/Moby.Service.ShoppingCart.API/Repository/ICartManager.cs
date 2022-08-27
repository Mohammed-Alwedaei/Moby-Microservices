using Moby.Services.ShoppingCart.API.Models;
using Moby.Services.ShoppingCart.API.Models.Dto;

namespace Moby.Services.ShoppingCart.API.Repository;

public interface ICartManager
{
    Task<CartDto> GetCartByUserIdAsync(string userId);

    Task<CartDto> CreateCartAsync(CartDto cart);

    Task<CartDto> UpdateCartAsync(CartDto cart);

    Task<bool> RemoveProductFromCartAsync(int cartDetailsId);

    Task<bool> ClearCartByIdAsync(string userId);

    Task<bool> ApplyCoupon(string userId, string couponCode);

    Task<bool> RemoveCoupon(string userId);
}
