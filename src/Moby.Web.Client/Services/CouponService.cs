using Moby.Web.Client.Services.IServices;
using Moby.Web.Shared;
using Moby.Web.Shared.Models;

namespace Moby.Web.Client.Services;

public class CouponService
{
    private readonly HttpClient _httpClient;

    public CouponService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    //public async Task<T> GetCouponByCodeNameAsync<T>(string couponCode)
    //{
    //    //return await SendAsync<T>(new HttpRequestModel
    //    //{
    //    //    HttpMethodTypes = SD.HttpMethodTypes.GET,
    //    //    Url = $"api/Coupons/{couponCode}"
    //    //});
    //}
}
