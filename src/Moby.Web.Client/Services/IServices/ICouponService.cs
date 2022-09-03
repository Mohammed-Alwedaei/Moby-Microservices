using Moby.Web.Shared.Models.Cart;

namespace Moby.Web.Client.Services.IServices;

public interface ICouponService
{
    Task<CouponDto> GetCouponByCodeNameAsync(string couponCode);
}
