using Moby.Web.Client.Services.IServices;
using Moby.Web.Shared;
using Moby.Web.Shared.Models;
using Moby.Web.Shared.Models.Cart;

namespace Moby.Web.Client.Services;

public class ShoppingCartService : BaseService, IShoppingCartService
{
    private readonly HttpClient _httpClient;

    public ShoppingCartService(HttpClient httpClient) : base(httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T> GetCartByUserIdAsync<T>(string userId)
    {
        return await SendAsync<T>(new HttpRequestModel
        {
            HttpMethodTypes = SD.HttpMethodTypes.GET,
            Url = $"api/carts/{userId}"
        });
    }

    public async Task<T> CreateCartAsync<T>(CartDto cartToCreate)
    {
        return await SendAsync<T>(new HttpRequestModel
        {
            HttpMethodTypes = SD.HttpMethodTypes.POST,
            Data = cartToCreate,
            Url = "api/carts"
        });
    }

    public async Task<T> UpdateCartAsync<T>(CartDto cartToUpdate)
    {
        return await SendAsync<T>(new HttpRequestModel
        {
            HttpMethodTypes = SD.HttpMethodTypes.PUT,
            Data = cartToUpdate,
            Url = "api/carts"
        });
    }

    public async Task<T> RemoveProductFromCartAsync<T>(int cartDetailsId)
    {
        return await SendAsync<T>(new HttpRequestModel
        {
            HttpMethodTypes = SD.HttpMethodTypes.DELETE,
            Url = "api/carts/cartDetailsId"
        });
    }
}
