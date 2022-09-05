namespace Moby.Web.Client.Pages.Products.Admin;

[Authorize]
public partial class Update
{
    [Inject]
    IProductService ProductService { get; set; }

    [Inject]
    NavigationManager NavigationManager { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "productId")]
    public int ProductId { get; set; }

    private ProductDto Product = new();


    protected override async Task OnInitializedAsync()
    {
        Product = await ProductService.GetProductByIdAsync(ProductId);
    }

    private async Task BeginProductUpdate()
    {
        var response = await ProductService.UpdateProductAsync(Product);

        if (response is true)
            NavigationManager.NavigateTo("/Products");
    }
}
