using Moby.Web.Shared;
using Moby.Web.Client.Services.IServices;
using Moby.Web.Shared.Models;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Moby.Web.Client.Services;

public class ProductService : BaseService, IProductService
{
    private readonly IHttpClientFactory _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    private readonly string _baseUrl;

    public ProductService(IHttpClientFactory httpClient, IConfiguration configuration, ITokenService tokenService) : base(httpClient, tokenService)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _tokenService = tokenService;

        _baseUrl = _configuration.GetValue<string>("GatewayUrl");

    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var client = await HttpClient(_baseUrl, ApiRoutes.Products);

        var response = await client.GetFromJsonAsync<ResponseDto>($"/api/products/{id}");

        if (response is null)
        {
            return new ProductDto();
        }

        return JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Results));
    }

    public async Task<List<ProductDto>> GetProductsAsync()
    {
        var client = await HttpClient(_baseUrl, ApiRoutes.Products);

        var response = await client
            .GetFromJsonAsync<ResponseDto>("/api/products");

        if (response is null)
        {
            return new List<ProductDto>();
        }

        return JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Results));
    }

    public async Task<bool> CreateProductAsync(ProductDto product)
    {
        var client = await HttpClient(_baseUrl, ApiRoutes.Products);

        var response = await client
            .PostAsJsonAsync("/api/products", product);

        if (response is null && response.IsSuccessStatusCode is false)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateProductAsync(ProductDto product)
    {
        var client = await HttpClient(_baseUrl, ApiRoutes.Products);

        var response = await client
            .PutAsJsonAsync("/api/products", product);

        if (response is null && response.IsSuccessStatusCode is false)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var client = await HttpClient(_baseUrl, ApiRoutes.Products);

        var response = await client
            .DeleteAsync($"/api/products/{id}");

        if (response is null && response.IsSuccessStatusCode is false)
        {
            return false;
        }

        return true;
    }
}