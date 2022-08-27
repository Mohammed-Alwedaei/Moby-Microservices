using Moby.Services.ShoppingCart.API.Models.Dto;

namespace Moby.Services.ShoppingCart.API.Repository;

public interface ICouponManager
{
    Task<CouponDto> GetCouponAsync(string couponCode);
}
