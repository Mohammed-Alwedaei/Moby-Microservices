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

        var shoppingCartResponse = await ShoppingCartService.RemoveProductFromCartAsync(cartDetailsId);

        if (shoppingCartResponse)
        {
            await JsRuntime.InvokeVoidAsync("alert", "Product Deleted", "success");
            await GetCartDtoByUserIdAsync();
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task GetCartDtoByUserIdAsync()
    {
        _isLoading = true;

        var cart = new CartDto();

        var userId = AuthenticationState
            .GetAuthenticationStateAsync()
            .Result.User.Claims
            .FirstOrDefault(c => c.Type == "sub")?.Value;

        try
        {
            var shoppingCartResponse = await ShoppingCartService.GetCartByUserIdAsync(userId);

            if (shoppingCartResponse is not null)
            {
                cart = shoppingCartResponse;

                if (cart.CartHeader is null && !cart.CartDetails.Any())
                {
                    cart = new();
                }
                else
                {
                    if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                    {
                        _couponDto = await CouponService.GetCouponByCodeNameAsync(cart.CartHeader.CouponCode);
                        var totalPrice = CalculateTotalCartPrice(cart.CartDetails);
                        cart.CartHeader.Total = totalPrice - _couponDto.DiscountAmount;
                        _hasDiscountCoupon = true;
                    }
                    else
                    {
                        cart.CartHeader.Total = CalculateTotalCartPrice(cart.CartDetails);
                        _hasDiscountCoupon = false;
                    }
                }
            }

            _isLoading = false;

            _cart = cart;
        }
        catch (Exception)
        {
            _isLoading = false;

            _cart = new();
        }
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
            var response = await ShoppingCartService.ApplyCouponAsync(userId, couponCode);

            if (response)
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

        var response = await ShoppingCartService.RemoveCouponAsync(userId);

        if (response)
        {
            _cart = new();
            await GetCartDtoByUserIdAsync();
        }
    }
}
