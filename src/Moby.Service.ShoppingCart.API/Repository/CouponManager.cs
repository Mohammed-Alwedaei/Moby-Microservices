using Moby.Services.ShoppingCart.API.Models.Dto;
using Newtonsoft.Json;

namespace Moby.Services.ShoppingCart.API.Repository;

public class CouponManager : ICouponManager
{
    private readonly HttpClient _httpClient;

    public CouponManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CouponDto> GetCouponAsync(string couponCode)
    {
        var response = await _httpClient.GetAsync($"/api/coupons/{couponCode}");

        var content = await response.Content.ReadAsStringAsync();

        var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);

        return (responseDto.IsSuccess 
            ? JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Results)) 
            : new CouponDto())!;
    }
}
