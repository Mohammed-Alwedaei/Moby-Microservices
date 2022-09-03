using Moby.Web.Shared;
using Moby.Web.Client.Services.IServices;
using Moby.Web.Shared.Models;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Moby.Web.Client.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClient;
    private readonly IConfiguration _configuration;

    public ProductService(IHttpClientFactory httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        
    }

    public async Task<ProductDto> GetProductByIdAsync(int id, string accessToken)
    {
        var response = await HttpClient(accessToken)
            .GetFromJsonAsync<ResponseDto>($"/api/products/{id}");

        if (response is null)
        {
            return new ProductDto();
        }

        return JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Results));
    }

    public async Task<List<ProductDto>> GetProductsAsync(string accessToken)
    {
        var response = await HttpClient(accessToken)
            .GetFromJsonAsync<ResponseDto>("/api/products");

        if (response is null)
        {
            return new List<ProductDto>();
        }

        return JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Results));
    }

    public async Task<bool> CreateProductAsync(ProductDto product, string accessToken)
    {
        var response = await HttpClient(accessToken)
            .PostAsJsonAsync("/api/products", product);

        if (response is null && response.IsSuccessStatusCode is false)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateProductAsync(ProductDto product, string accessToken)
    {
        var response = await HttpClient(accessToken)
            .PutAsJsonAsync("/api/products", product);

        if (response is null && response.IsSuccessStatusCode is false)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteProductAsync(int id, string accessToken)
    {
        var response = await HttpClient(accessToken)
            .DeleteAsync($"/api/products/{id}");

        if (response is null && response.IsSuccessStatusCode is false)
        {
            return false;
        }

        return true;
    }

    private HttpClient HttpClient(string accessToken)
    {
        var client = _httpClient.CreateClient("ProductClient");

        client.BaseAddress = new Uri(_configuration["ServicesUrls:Products.API"]);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return client;
    }
}