using System.Net.Http.Json;
using Moby.Web.Client.Services.IServices;
using Moby.Web.Shared;
using Moby.Web.Shared.Models;
using Moby.Web.Shared.Models.Cart;
using Newtonsoft.Json;

namespace Moby.Web.Client.Services;

public class CouponService : BaseService, ICouponService
{
    private readonly IHttpClientFactory _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    private readonly string _baseUrl;
    private readonly string _serviceName;

    public CouponService(IHttpClientFactory httpClient, 
        IConfiguration configuration, 
        ITokenService tokenService) : base(httpClient, tokenService)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _tokenService = tokenService;

        _baseUrl = _configuration.GetValue<string>("GatewayUrl");

        _serviceName = "Coupons";
    }

    public async Task<CouponDto> GetCouponByCodeNameAsync(string couponCode)
    {
        var client = await HttpClient(_baseUrl, _serviceName);

        var response = await client.GetFromJsonAsync<ResponseDto>($"/api/coupons/{couponCode}");

        if (!response.IsSuccess)
        {
            return new CouponDto();
        }

        return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Results));

    }
}
