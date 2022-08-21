using Moby.Service.ShoppingCart.API.Models.Dto;

namespace Moby.Service.ShoppingCart.API.Repository;

public class CartManager : ICartManager
{
    public async Task<CartDto> GetCartByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<CartDto> CreateCartAsync(CartDto cart)
    {
        throw new NotImplementedException();
    }

    public async Task<CartDto> UpdateCartAsync(CartDto cart)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveProductFromCartAsync(int cartDetailsId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ClearCartByIdAsync(int cartId)
    {
        throw new NotImplementedException();
    }
}
