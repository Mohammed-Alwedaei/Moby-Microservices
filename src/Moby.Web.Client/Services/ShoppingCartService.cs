namespace Moby.Web.Client.Services;

public class ShoppingCartService : BaseService, IShoppingCartService
{
    private readonly IHttpClientFactory _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public ShoppingCartService(IHttpClientFactory httpClient, 
        IConfiguration configuration, 
        ITokenService tokenService) : base(httpClient, tokenService, configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Get a cart by user id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>CartDto</returns>
    public async Task<CartDto> GetCartByUserIdAsync(string userId)
    {
        var client = await HttpClient();
        var response = await client.GetFromJsonAsync<ResponseDto>($"/api/carts/{userId}");

        if (response is null)
        {
            return new CartDto();
        }

        return JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Results));
    }

    /// <summary>
    /// Create cart
    /// </summary>
    /// <param name="cartToCreate"></param>
    /// <returns>An indicator whether the cart is created or not</returns>
    public async Task<bool> CreateCartAsync(PostCartDto cartToCreate)
    {
        var client = await HttpClient();

        var response = await client.PostAsJsonAsync("/api/carts", cartToCreate);

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    /// <summary>
    /// Update cart
    /// </summary>
    /// <param name="cartToUpdate"></param>
    /// <returns>An indicator whether the cart is updated or not</returns>
    public async Task<bool> UpdateCartAsync(CartDto cartToUpdate)
    {
        var client = await HttpClient();
        var response = await client.PutAsJsonAsync("/api/carts", cartToUpdate);

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    /// <summary>
    /// Remove product from cart by cart details id
    /// </summary>
    /// <param name="cartDetailsId"></param>
    /// <returns>An indicator whether the products is removed from cart or not</returns>
    public async Task<bool> RemoveProductFromCartAsync(int cartDetailsId)
    {
        var client = await HttpClient();
        var response = await client.DeleteAsync($"/api/carts/{cartDetailsId}");

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    /// <summary>
    /// Apply a coupon for a cart header by user id and coupon code
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="couponCode"></param>
    /// <returns>An indicator whether the coupon is applied or not</returns>
    public async Task<bool> ApplyCouponAsync(string userId, string couponCode)
    {
        var client = await HttpClient();
        var response = await client.PostAsJsonAsync($"/api/cartcoupons/{userId}/{couponCode}", "");

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    /// <summary>
    /// Remove coupon from cart header by user id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>An indicator whether the coupon is removed or not</returns>
    public async Task<bool> RemoveCouponAsync(string userId)
    {
        var client = await HttpClient();
        var response = await client.PostAsJsonAsync($"/api/cartcoupons/{userId}", "");

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }

    /// <summary>
    /// Checkout a cart 
    /// </summary>
    /// <param name="cartHeader"></param>
    /// <returns>An indicator whether the checkout is processed or not</returns>
    public async Task<bool> CheckoutAsync(CartHeaderDto cartHeader)
    {
        var client = await HttpClient();
        var response = await client.PostAsJsonAsync($"/api/checkouts", cartHeader);

        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }
}
