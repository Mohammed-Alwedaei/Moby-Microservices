using Moby.Web.Shared.Models.Cart;

namespace Moby.Web.Client.Services.IServices;

public interface IShoppingCartService
{
    Task<T> GetCartByUserIdAsync<T>(string userId);

    Task<T> CreateCartAsync<T>(CartDto cart);

    Task<T> UpdateCartAsync<T>(CartDto cart);

    Task<T> RemoveProductFromCartAsync<T>(int cartDetailsId);
}
