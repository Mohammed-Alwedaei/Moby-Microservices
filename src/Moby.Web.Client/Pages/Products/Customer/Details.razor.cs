namespace Moby.Web.Client.Pages.Products.Customer;

public partial class Details
{
    [Inject]
    IProductService ProductService { get; set; }

    [Inject]
    IShoppingCartService ShoppingCartService { get; set; }

    [Inject]
    AuthenticationStateProvider AuthenticationState { get; set; }

    [Inject]
    NavigationManager NavigationManager { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "productid")]

    public int ProductId { get; set; }

    private ProductDto _product = new();

    private CartDetails Cart = new();

    /// <summary>
    /// Call BeginAddToCartFlow() to get a product on component initialization
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        _product = await ProductService.GetProductByIdAsync(ProductId);
    }

    /// <summary>
    /// Add Product to cart
    /// </summary>
    /// <returns></returns>
    private async Task BeginAddToCartFlow()
    {
        var cart = new PostCartDto
        {
            CartHeader = new()
            {
                UserId = AuthenticationState
                    .GetAuthenticationStateAsync()
                    .Result
                    .User
                    .Claims
                    .FirstOrDefault(u => u.Type == "sub")
                    !.Value
            }
        };

        var productToAddResponse = await ProductService.GetProductByIdAsync(ProductId);

        var cartDetails = new PostCartDetailsDto
        {
            ProductId = ProductId,
            Count = Cart.Count
        };

        var productToAdd = new ProductDto();

        if (productToAddResponse is not null)
        {
            productToAdd = productToAddResponse;
        }

        cartDetails.Product = productToAdd;

        List<PostCartDetailsDto> cartDetailsDto = new();

        cartDetailsDto.Add(cartDetails);

        cart.CartDetails = cartDetailsDto;

        var shoppingCartResponse = await ShoppingCartService.CreateCartAsync(cart);

        if (shoppingCartResponse)
        {
            NavigationManager.NavigateTo("/");
        }
    }

    class CartDetails
    {
        public int Count { get; set; } = 1;
    }
}
