using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Moby.Web.Client.Services;
using Moby.Web.Client.Services.IServices;
using Moby.Web.Shared.Models;
using Moby.Web.Shared.Models.Cart;
using Newtonsoft.Json;

namespace Moby.Web.Client.Pages.Cart;

public partial class Index
{
    [Inject]
    IProductService ProductService { get; set; }

    [Inject]
    IShoppingCartService ShoppingCartService { get; set; }

    [Inject] 
    AuthenticationStateProvider AuthenticationState { get; set; }

    [Inject] 
    IJSRuntime JsRuntime { get; set; }

    private CartDto _cart = new();

    private bool _isLoading;

    protected override async Task OnInitializedAsync()
    {
        _cart = await GetCartDtoByUserIdAsync();
    }

    private async Task RemoveProductFromCart(int cartDetailsId)
    {
        var userId = AuthenticationState.GetAuthenticationStateAsync()
            .Result.User.Claims.
            FirstOrDefault(c => c.Type == "sub")?.Value;

        var shoppingCartResponse = await ShoppingCartService.RemoveProductFromCartAsync<ResponseDto>(cartDetailsId);

        if (shoppingCartResponse.Results is not null || shoppingCartResponse.IsSuccess)
            await JsRuntime.InvokeVoidAsync("alert", "Product Deleted", "success");

        await InvokeAsync(StateHasChanged);
    }

    private async Task<CartDto> GetCartDtoByUserIdAsync()
    {
        _isLoading = true;

        var cart = new CartDto();

        var userId = AuthenticationState.GetAuthenticationStateAsync()
            .Result.User.Claims.
            FirstOrDefault(c => c.Type == "sub")?.Value;

        var shoppingCartResponse = await ShoppingCartService.GetCartByUserIdAsync<ResponseDto>(userId);

        if (shoppingCartResponse.Results is not null || shoppingCartResponse.IsSuccess)
            cart = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(shoppingCartResponse.Results));

        cart.CartHeader.Total = CalculateTotalCartPrice(cart.CartDetails);

        _isLoading = false;

        return cart;
    }

    private decimal CalculateTotalCartPrice(IEnumerable<CartDetailsDto> cartDetailsDtos)
    {
        var cartTotal = 0m;

        foreach (var detail in cartDetailsDtos)
        {
            cartTotal += (detail.Product.Price * detail.Count);
        }

        return cartTotal;
    }
}
