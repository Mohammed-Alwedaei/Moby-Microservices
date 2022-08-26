using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Moby.Web.Client.Services;
using Moby.Web.Client.Services.IServices;
using Moby.Web.Shared.Models;
using Moby.Web.Shared.Models.Cart;
using Newtonsoft.Json;

namespace Moby.Web.Client.Pages.Cart;

public partial class Checkout
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
    NavigationManager NavigationManager { get; set; }

    private CartDto _cart = new CartDto();

    private CouponDto _couponDto = new CouponDto();

    private bool _hasDiscountCoupon = false;

    private bool _isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        await GetCartDtoByUserIdAsync();
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

    private async Task<CouponDto> GetCoupon(string couponCode)
    {
        var response = await CouponService.GetCouponByCodeNameAsync<ResponseDto>(couponCode);

        var coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Results));

        return coupon;
    }

    private async Task BeginCheckoutFlow()
    {
        var cart = _cart;
        var response = await ShoppingCartService.CheckoutAsync<ResponseDto>(_cart.CartHeader);

        if (response is null || response.IsSuccess is false)
            return;

        NavigationManager.NavigateTo("/cart/orders/confirmed");
    }
}
