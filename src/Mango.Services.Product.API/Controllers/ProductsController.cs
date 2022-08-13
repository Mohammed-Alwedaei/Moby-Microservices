using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moby.Services.Product.API.Models.Dto;
using Moby.Services.Product.API.Repository.IRepository;

namespace Moby.Services.Product.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        protected ResponseDto _response;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _response = new();
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<object> GetProductById(int id)
        {
            try
            {
                var productsFromDb = await _productRepository.GetProductByIdAsync(id);
                _response.Results = productsFromDb;
            }
            catch (Exception exception)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>
                {
                    exception.Message
                };
            }

            return _response;
        }

        [HttpGet]
        public async Task<object> GetProducts()
        {
            try
            {
                var productsFromDb = await _productRepository.GetProductsAsync();
                _response.Results = productsFromDb;
            }
            catch (Exception exception)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>
                {
                    exception.Message
                };
            }

            return _response;
        }

        [HttpPost]
        public async Task<object> CreateProduct([FromBody] ProductDto product)
        {
            try
            {
                var productsFromDb = await _productRepository.CreateProductAsync(product);
                _response.Results = productsFromDb;
            }
            catch (Exception exception)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>
                {
                    exception.Message
                };
            }

            return _response;
        }

        [HttpPut]
        public async Task<object> UpdateProduct([FromBody] ProductDto product)
        {
            try
            {
                var productsFromDb = await _productRepository.UpdateProductAsync(product);
                _response.Results = productsFromDb;
            }
            catch (Exception exception)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>
                {
                    exception.Message
                };
            }

            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<object> DeleteProduct(int id)
        {
            try
            {
                var productsFromDb = await _productRepository.DeleteProductByIdAsync(id);
                _response.Results = productsFromDb;
            }
            catch (Exception exception)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>
                {
                    exception.Message
                };
            }

            return _response;
        }
    }
}
