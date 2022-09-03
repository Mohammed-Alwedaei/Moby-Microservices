using Moby.Web.Shared.Models.Cart;
using Moby.Web.Shared.Models.Cart.Post;

namespace Moby.Web.Client.Services.IServices;

public interface IShoppingCartService
{
    Task<CartDto> GetCartByUserIdAsync(string userId, string accessToken);

    Task<bool> CreateCartAsync(PostCartDto cart, string accessToken);

    //Task<T> UpdateCartAsync<T>(CartDto cart);

    //Task<T> RemoveProductFromCartAsync<T>(int cartDetailsId);

    //Task<T> ApplyCouponAsync<T>(string userId, string couponCode);

    //Task<T> RemoveCouponAsync<T>(string userId);

    //Task<T> CheckoutAsync<T>(CartHeaderDto cartHeader);
}
