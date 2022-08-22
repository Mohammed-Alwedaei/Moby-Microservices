using Moby.Service.ShoppingCart.API.Models.Dto;

namespace Moby.Service.ShoppingCart.API.Repository;

public interface ICartManager
{
    Task<CartDto> GetCartByUserIdAsync(string userId);

    Task<CartDto> CreateCartAsync(CartDto cart);

    Task<CartDto> UpdateCartAsync(CartDto cart);

    Task<bool> RemoveProductFromCartAsync(int cartDetailsId);

    Task<bool> ClearCartByIdAsync(int cartId);
}
