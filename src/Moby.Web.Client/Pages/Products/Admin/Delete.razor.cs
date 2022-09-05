namespace Moby.Web.Client.Pages.Products.Admin;

[Authorize]
public partial class Delete
{
    [Inject]
    IProductService ProductService { get; set; }

    [Inject]
    NavigationManager NavigationManager { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "productId")]
    public int ProductId { get; set; }

    private ProductDto Product = new();

    /// <summary>
    /// Get a product to delete on component initialization
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        Product = await ProductService.GetProductByIdAsync(ProductId);
    }

    /// <summary>
    /// Begin product delete process on delete button click
    /// </summary>
    /// <returns></returns>
    private async Task BeginProductDelete()
    {
        var response = await ProductService.DeleteProductAsync(ProductId);

        if (response)
            NavigationManager.NavigateTo("/Products");
    }
}
