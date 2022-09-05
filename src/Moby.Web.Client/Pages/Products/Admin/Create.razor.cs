namespace Moby.Web.Client.Pages.Products.Admin;

[Authorize]
public partial class Create
{
    [Inject]
    IProductService ProductService { get; set; }

    [Inject]
    NavigationManager NavigationManager { get; set; }

    private ProductDto Product = new();

    private async Task BeginProductCreate()
    {
        var response = await ProductService.CreateProductAsync(Product);

        if (response)
            NavigationManager.NavigateTo("/Products?alert=success&title=Product Created");
    }
}
