using Moby.Web.Shared.Models.Cart;
using Moby.Web.Shared.Models.Cart.Post;

namespace Moby.Web.Client.Services.IServices;

public interface IShoppingCartService
{
    Task<CartDto> GetCartByUserIdAsync(string userId);

    Task<bool> CreateCartAsync(PostCartDto cart);

    Task<bool> UpdateCartAsync(CartDto cart);

    Task<bool> RemoveProductFromCartAsync(int cartDetailsId);

    Task<bool> ApplyCouponAsync(string userId, string couponCode);

    Task<bool> RemoveCouponAsync(string userId);

    Task<bool> CheckoutAsync(CartHeaderDto cartHeader);
}
