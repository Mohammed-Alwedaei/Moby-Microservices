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

    private bool _hasErrors = false;
    private string _errorMessage = "";

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

        var shoppingCartResponse = await ShoppingCartService.GetCartByUserIdAsync(userId);

        if (shoppingCartResponse is not null)
            cart = shoppingCartResponse;

        if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
        {
            _couponDto = await GetCoupon(cart.CartHeader.CouponCode);
            var totalPrice = CalculateTotalCartPrice(cart.CartDetails);

            cart.CartHeader.Total = totalPrice - _couponDto.DiscountAmount;
            cart.CartHeader.TotalAfterDiscount = _couponDto.DiscountAmount;
            _hasDiscountCoupon = true;
        }
        else
        {
            cart.CartHeader.Total = CalculateTotalCartPrice(cart.CartDetails);
            cart.CartHeader.TotalAfterDiscount = 0;
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
        var coupon = await CouponService.GetCouponByCodeNameAsync(couponCode);

        return coupon;
    }

    private async Task BeginCheckoutFlow()
    {
        var cart = _cart;
        _hasErrors = false;

        var response = await ShoppingCartService.CheckoutAsync(_cart.CartHeader);

        if (!response)
        {
            _hasErrors = true;

            return;
        }

        NavigationManager.NavigateTo("/cart/orders/confirmed");
    }
}
