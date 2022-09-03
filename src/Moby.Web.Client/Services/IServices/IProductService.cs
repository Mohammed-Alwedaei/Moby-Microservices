using Moby.Web.Shared.Models;

namespace Moby.Web.Client.Services.IServices;

public interface IProductService
{
    Task<ProductDto> GetProductByIdAsync(int id, string accessToken);

    Task<List<ProductDto>> GetProductsAsync(string accessToken);

    Task<bool> CreateProductAsync(ProductDto product, string accessToken);

    Task<bool> UpdateProductAsync(ProductDto product, string accessToken);

    Task<bool> DeleteProductAsync(int id, string accessToken);
}