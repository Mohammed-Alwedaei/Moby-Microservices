using Moby.Services.Coupon.API.Models.Dtos;

namespace Moby.Services.Coupon.API.Repository;

public interface ICouponManager
{
    Task<CouponDto> GetCouponByCodeAsync(string code);
}
