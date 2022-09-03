using Moby.Web.Client.Services.IServices;
using Moby.Web.Shared.Models;
using Moby.Web.Shared.Models.Cart;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Moby.Web.Shared.Models.Cart.Post;
using Newtonsoft.Json;

namespace Moby.Web.Client.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IHttpClientFactory _httpClient;
    private readonly IConfiguration _configuration;

    public ShoppingCartService(IHttpClientFactory httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;

    }

    public async Task<CartDto> GetCartByUserIdAsync(string userId, string accessToken)
    {
        var response = await HttpClient(accessToken)
            .GetFromJsonAsync<ResponseDto>($"/api/carts/{userId}");

        if (response is null)
        {
            return new CartDto();
        }

        return JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Results));
    }

    public async Task<bool> CreateCartAsync(PostCartDto cartToCreate, string accessToken)
    {
        var response = await HttpClient(accessToken).PostAsJsonAsync<PostCartDto>("/api/carts", cartToCreate);

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    //public async Task<T> UpdateCartAsync<T>(CartDto cartToUpdate)
    //{
    //    return await SendAsync<T>(new HttpRequestModel
    //    {
    //        HttpMethodTypes = SD.HttpMethodTypes.PUT,
    //        Data = cartToUpdate,
    //        Url = "api/carts"
    //    });
    //}

    //public async Task<T> RemoveProductFromCartAsync<T>(int cartDetailsId)
    //{
    //    return await SendAsync<T>(new HttpRequestModel
    //    {
    //        HttpMethodTypes = SD.HttpMethodTypes.DELETE,
    //        Url = $"api/carts/{cartDetailsId}"
    //    });
    //}

    //public async Task<T> ApplyCouponAsync<T>(string userId, string couponCode)
    //{
    //    return await SendAsync<T>(new HttpRequestModel
    //    {
    //        HttpMethodTypes = SD.HttpMethodTypes.POST,
    //        Url = $"api/CartCoupons/{userId}/{couponCode}"
    //    });
    //}

    //public async Task<T> RemoveCouponAsync<T>(string userId)
    //{
    //    return await SendAsync<T>(new HttpRequestModel
    //    {
    //        HttpMethodTypes = SD.HttpMethodTypes.POST,
    //        Url = $"api/CartCoupons/{userId}"
    //    });
    //}

    //public async Task<T> CheckoutAsync<T>(CartHeaderDto cartHeader)
    //{
    //    return await SendAsync<T>(new HttpRequestModel
    //    {
    //        HttpMethodTypes = SD.HttpMethodTypes.POST,
    //        Data = cartHeader,
    //        Url = "api/checkouts"
    //    });
    //}

    private HttpClient HttpClient(string accessToken)
    {
        var client = _httpClient.CreateClient("ProductClient");

        client.BaseAddress = new Uri(_configuration["ServicesUrls:ShoppingCart.API"]);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return client;
    }
}
