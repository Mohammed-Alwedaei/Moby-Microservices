using Moby.Web.Client.Models;
using Moby.Web.Client.Services.IServices;

namespace Moby.Web.Client.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetProductByIdAsync<T>(int id)
        {
            return await SendAsync<T>(new HttpRequestModel
            {
                 HttpMethodTypes = SD.HttpMethodTypes.GET,
                 Url = $"{SD.ProductsBaseApi}/api/products/{id}",
                 AccessToken = ""
            });
        }

        public async Task<T> GetProductsAsync<T>()
        {
            return await SendAsync<T>(new HttpRequestModel
            {
                HttpMethodTypes = SD.HttpMethodTypes.GET,
                Url = $"{SD.ProductsBaseApi}/api/products",
                AccessToken = ""
            });
        }

        public async Task<T> CreateProductAsync<T>(ProductDto product)
        {
            return await SendAsync<T>(new HttpRequestModel
            {
                HttpMethodTypes = SD.HttpMethodTypes.POST,
                Url = $"{SD.ProductsBaseApi}/api/products",
                Data = product,
                AccessToken = ""
            });
        }

        public async Task<T> UpdateProductAsync<T>(ProductDto product)
        {
            return await SendAsync<T>(new HttpRequestModel
            {
                HttpMethodTypes = SD.HttpMethodTypes.PUT,
                Url = $"{SD.ProductsBaseApi}/api/products",
                Data = product,
                AccessToken = ""
            });
        }

        public async Task<T> DeleteProductAsync<T>(int id)
        {
            return await SendAsync<T>(new HttpRequestModel
            {
                HttpMethodTypes = SD.HttpMethodTypes.DELETE,
                Url = $"{SD.ProductsBaseApi}/api/products/{id}",
                AccessToken = ""
            });
        }
    }
}
