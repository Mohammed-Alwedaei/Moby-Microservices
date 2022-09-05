namespace Moby.Web.Client.Pages.Products.Customer;

public partial class Index
{
    [Inject]
    IProductService ProductService { get; set; }

    [Inject]
    IJSRuntime JsRuntime { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "alert")]
    public string Alert { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "title")]
    public string Title { get; set; }

    private List<ProductDto> Products = new();

    /// <summary>
    /// Get a list of products on component initialization
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        Products = await ProductService.GetProductsAsync();
    }

    /// <summary>
    /// Check if there is an alert to be shown based on the query parameters
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            if (string.IsNullOrEmpty(Alert) && string.IsNullOrEmpty(Title))
                return;
            else
            {
                Title = Title.Replace("%", " ");
                await JsRuntime.InvokeVoidAsync("alert", Title, Alert);
            }
    }
}
