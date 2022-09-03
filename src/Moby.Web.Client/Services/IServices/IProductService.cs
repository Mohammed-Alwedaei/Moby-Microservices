using Moby.Web.Shared.Models;

namespace Moby.Web.Client.Services.IServices;

public interface IProductService
{
    Task<ProductDto> GetProductByIdAsync(int id);

    Task<List<ProductDto>> GetProductsAsync();

    Task<bool> CreateProductAsync(ProductDto product);

    Task<bool> UpdateProductAsync(ProductDto product);

    Task<bool> DeleteProductAsync(int id);
}