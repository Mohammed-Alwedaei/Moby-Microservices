using AutoMapper;
using Moby.Services.Product.API.Models;
using Moby.Services.Product.API.Models.Dto;

namespace Moby.Services.Product.API.Mapper
{
    public class MapperConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<ProductModel, ProductDto>().ReverseMap();
            });
        }
    }
}
