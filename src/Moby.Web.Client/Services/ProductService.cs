namespace Moby.Web.Client.Services;

public class ProductService : BaseService, IProductService
{
    private readonly IHttpClientFactory _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public ProductService(IHttpClientFactory httpClient, 
        IConfiguration configuration, 
        ITokenService tokenService) : base(httpClient, tokenService, configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Get a product by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>ProductDto</returns>
    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var client = await HttpClient();

        var response = await client.GetFromJsonAsync<ResponseDto>($"/api/products/{id}");

        if (response is null)
        {
            return new ProductDto();
        }

        return JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Results));
    }

    /// <summary>
    /// Get a list of products
    /// </summary>
    /// <returns>List of ProductDto</returns>
    public async Task<List<ProductDto>> GetProductsAsync()
    {
        var client = await HttpClient();

        var response = await client
            .GetFromJsonAsync<ResponseDto>("/api/products");

        if (response is null)
        {
            return new List<ProductDto>();
        }

        return JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Results));
    }

    /// <summary>
    /// Create a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns>An indicator whether the products is updated or not</returns>
    public async Task<bool> CreateProductAsync(ProductDto product)
    {
        var client = await HttpClient();

        var response = await client
            .PostAsJsonAsync("/api/products", product);

        if (response is null && response.IsSuccessStatusCode is false)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Update a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns>An indicator whether the products is updated or not</returns>
    public async Task<bool> UpdateProductAsync(ProductDto product)
    {
        var client = await HttpClient();

        var response = await client
            .PutAsJsonAsync("/api/products", product);

        if (response is null && response.IsSuccessStatusCode is false)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Delete a product by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>An indicator whether the products is updated or not</returns>
    public async Task<bool> DeleteProductAsync(int id)
    {
        var client = await HttpClient();

        var response = await client
            .DeleteAsync($"/api/products/{id}");

        if (response is null && response.IsSuccessStatusCode is false)
        {
            return false;
        }

        return true;
    }
}