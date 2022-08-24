namespace Moby.Web.Client.Services.IServices;

public interface ICouponService
{
    Task<T> GetCouponByCodeNameAsync<T>(string couponCode);
}
