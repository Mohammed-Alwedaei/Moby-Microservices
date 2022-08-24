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
    ICouponService CouponService { get; set; }

    [Inject]
    AuthenticationStateProvider AuthenticationState { get; set; }

    [Inject]
    IJSRuntime JsRuntime { get; set; }

    private CartCouponModel CartCouponCode = new();

    private CouponDto _couponDto = new CouponDto();

    private bool _hasDiscountCoupon;

    private CartDto _cart = new();

    private bool _isLoading;

    protected override async Task OnInitializedAsync()
    {
        await GetCartDtoByUserIdAsync();
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

    private async Task GetCartDtoByUserIdAsync()
    {
        _isLoading = true;

        var cart = new CartDto();

        var userId = AuthenticationState.GetAuthenticationStateAsync()
            .Result.User.Claims.
            FirstOrDefault(c => c.Type == "sub")?.Value;

        var shoppingCartResponse = await ShoppingCartService.GetCartByUserIdAsync<ResponseDto>(userId);

        if (shoppingCartResponse.Results is not null || shoppingCartResponse.IsSuccess)
            cart = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(shoppingCartResponse.Results));

        if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
        {
            _couponDto = await GetCoupon(cart.CartHeader.CouponCode);
            var totalPrice = CalculateTotalCartPrice(cart.CartDetails);
            cart.CartHeader.Total = totalPrice - _couponDto.DiscountAmount;
            _hasDiscountCoupon = true;
        }
        else
        {
            cart.CartHeader.Total = CalculateTotalCartPrice(cart.CartDetails);
            _hasDiscountCoupon = false;
        }

        _isLoading = false;

        _cart = cart;
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

    class CartCouponModel
    {
        public string CouponCode { get; set; }
    }

    private async Task ApplyCouponToUser()
    {
        var userId = AuthenticationState.GetAuthenticationStateAsync()
            .Result.User.Claims.
            FirstOrDefault(c => c.Type == "sub")?.Value;

        var couponCode = CartCouponCode.CouponCode;

        if (!string.IsNullOrEmpty(couponCode))
        {
            var response = await ShoppingCartService.ApplyCouponAsync<ResponseDto>(userId, couponCode);


            if (response is not null || response.IsSuccess)
            {
                _cart = new();
                await GetCartDtoByUserIdAsync();
            }
        }
    }

    private async Task RemoveCouponFromUser()
    {
        var userId = AuthenticationState.GetAuthenticationStateAsync()
            .Result.User.Claims.
            FirstOrDefault(c => c.Type == "sub")?.Value;

        var response = await ShoppingCartService.RemoveCouponAsync<ResponseDto>(userId);

        if (response is not null || response.IsSuccess)
        {
            _cart = new();
            await GetCartDtoByUserIdAsync();
        }
    }

    private async Task<CouponDto> GetCoupon(string couponCode)
    {
        var response = await CouponService.GetCouponByCodeNameAsync<ResponseDto>(couponCode);

        var coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Results));

        return coupon;
    }
}
