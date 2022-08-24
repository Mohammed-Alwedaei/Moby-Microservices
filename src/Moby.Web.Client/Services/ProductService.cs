using Moby.Web.Shared;
using Moby.Web.Client.Services.IServices;
using Moby.Web.Shared.Models;

namespace Moby.Web.Client.Services;

public class ProductService : BaseService, IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient) : base(httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T> GetProductByIdAsync<T>(int id)
    {
        return await SendAsync<T>(new HttpRequestModel
        {
            HttpMethodTypes = SD.HttpMethodTypes.GET,
            Url = $"api/products/{id}"
        });
    }

    public async Task<T> GetProductsAsync<T>()
    {
        return await SendAsync<T>(new HttpRequestModel
        {
            HttpMethodTypes = SD.HttpMethodTypes.GET,
            Url = "api/products"
        });
    }

    public async Task<T> CreateProductAsync<T>(ProductDto product)
    {
        return await SendAsync<T>(new HttpRequestModel
        {
            HttpMethodTypes = SD.HttpMethodTypes.POST,
            Url = $"api/products",
            Data = product
        });
    }

    public async Task<T> UpdateProductAsync<T>(ProductDto product)
    {
        return await SendAsync<T>(new HttpRequestModel
        {
            HttpMethodTypes = SD.HttpMethodTypes.PUT,
            Url = $"api/products",
            Data = product
        });
    }

    public async Task<T> DeleteProductAsync<T>(int id)
    {
        return await SendAsync<T>(new HttpRequestModel
        {
            HttpMethodTypes = SD.HttpMethodTypes.DELETE,
            Url = $"api/products/{id}"
        });
    }
}