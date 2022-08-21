using Moby.Web.Client.Models;

namespace Moby.Web.Client.Services.IServices;

public interface IProductService
{
    Task<T> GetProductByIdAsync<T>(int id);

    Task<T> GetProductsAsync<T>();

    Task<T> CreateProductAsync<T>(ProductDto product);

    Task<T> UpdateProductAsync<T>(ProductDto product);

    Task<T> DeleteProductAsync<T>(int id);
}