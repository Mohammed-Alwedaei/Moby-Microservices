using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moby.Services.Product.API.DbContexts;
using Moby.Services.Product.API.Models;
using Moby.Services.Product.API.Models.Dto;
using Moby.Services.Product.API.Repository.IRepository;

namespace Moby.Services.Product.API.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ProductRepository(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var productFromDb = await _db.Products
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        return _mapper.Map<ProductDto>(productFromDb);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        var productsFromDb = await _db.Products.ToListAsync();

        return _mapper.Map<IEnumerable<ProductDto>>(productsFromDb);
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto product)
    {
        var toBeCreatedProduct = _mapper.Map<ProductDto, ProductModel>(product);

        _db.Products.Add(toBeCreatedProduct);
        await _db.SaveChangesAsync();

        var createdProduct = _mapper.Map<ProductModel, ProductDto>(toBeCreatedProduct);

        return createdProduct;
    }

    public async Task<ProductDto> UpdateProductAsync(ProductDto product)
    {
        var toBeUpdatedProduct = _mapper.Map<ProductDto, ProductModel>(product);

        _db.Products.Update(toBeUpdatedProduct);
        await _db.SaveChangesAsync();

        var updatedProduct = _mapper.Map<ProductModel, ProductDto>(toBeUpdatedProduct);

        return updatedProduct;
    }

    public async Task<bool> DeleteProductByIdAsync(int id)
    {
        try
        {
            var toBeDeletedProduct = await _db.Products
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (toBeDeletedProduct is null) return false;

            _db.Products.Remove(toBeDeletedProduct);

            await _db.SaveChangesAsync();

            return true;
        }

        catch (Exception)
        {
            return false;       
        }
    }
}