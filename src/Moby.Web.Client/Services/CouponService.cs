namespace Moby.Web.Client.Services;

public class CouponService : BaseService, ICouponService
{
    private readonly IHttpClientFactory _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public CouponService(IHttpClientFactory httpClient, 
        IConfiguration configuration, 
        ITokenService tokenService) : base(httpClient, tokenService, configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Get coupon by coupon code 
    /// </summary>
    /// <param name="couponCode"></param>
    /// <returns>CouponDto</returns>
    public async Task<CouponDto> GetCouponByCodeNameAsync(string couponCode)
    {
        var client = await HttpClient();

        var response = await client.GetFromJsonAsync<ResponseDto>($"/api/coupons/{couponCode}");

        if (!response.IsSuccess)
        {
            return new CouponDto();
        }

        return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Results));
    }
}
