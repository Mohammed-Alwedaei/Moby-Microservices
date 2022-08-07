using Moby.Services.Product.API.Models.Dto;

namespace Moby.Services.Product.API.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();

        Task<ProductDto> GetProductByIdAsync(int id);

        Task<ProductDto> CreateProductAsync(ProductDto product);
        Task<ProductDto> UpdateProductAsync(ProductDto product);

        Task<bool> DeleteProductByIdAsync(int id);
    }
}
