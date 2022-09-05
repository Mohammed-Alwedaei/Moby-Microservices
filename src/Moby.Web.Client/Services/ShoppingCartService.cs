using Moby.Web.Client.Services.IServices;
using Moby.Web.Shared.Models;
using Moby.Web.Shared.Models.Cart;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Moby.Web.Shared;
using Moby.Web.Shared.Models.Cart.Post;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Moby.Web.Client.Services;

public class ShoppingCartService : BaseService, IShoppingCartService
{
    private readonly IHttpClientFactory _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    private readonly string _baseUrl;
    private readonly string _serviceName;

    public ShoppingCartService(IHttpClientFactory httpClient, IConfiguration configuration, ITokenService tokenService) : base(httpClient, tokenService)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _tokenService = tokenService;

        _baseUrl = _configuration.GetValue<string>("GatewayUrl");

        _serviceName = ApiRoutes.Carts;
    }

    public async Task<CartDto> GetCartByUserIdAsync(string userId)
    {
        var client = await HttpClient(_baseUrl, _serviceName);
        var response = await client.GetFromJsonAsync<ResponseDto>($"/api/carts/{userId}");

        if (response is null)
        {
            return new CartDto();
        }

        return JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Results));
    }

    public async Task<bool> CreateCartAsync(PostCartDto cartToCreate)
    {
        var client = await HttpClient(_baseUrl, _serviceName);

        var response = await client.PostAsJsonAsync("/api/carts", cartToCreate);

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    public async Task<bool> UpdateCartAsync(CartDto cartToUpdate)
    {
        var client = await HttpClient(_baseUrl, _serviceName);
        var response = await client.PutAsJsonAsync("/api/carts", cartToUpdate);

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    public async Task<bool> RemoveProductFromCartAsync(int cartDetailsId)
    {
        var client = await HttpClient(_baseUrl, _serviceName);
        var response = await client.DeleteAsync($"/api/carts/{cartDetailsId}");

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    public async Task<bool> ApplyCouponAsync(string userId, string couponCode)
    {
        var client = await HttpClient(_baseUrl, _serviceName);
        var response = await client.PostAsJsonAsync($"/api/cartcoupons/{userId}/{couponCode}", "");

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    public async Task<bool> RemoveCouponAsync(string userId)
    {
        var client = await HttpClient(_baseUrl, _serviceName);
        var response = await client.PostAsJsonAsync($"/api/cartcoupons/{userId}", "");

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    public async Task<bool> CheckoutAsync(CartHeaderDto cartHeader)
    {
        var client = await HttpClient(_baseUrl, _serviceName);
        var response = await client.PostAsJsonAsync($"/api/checkouts", cartHeader);

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }
}
