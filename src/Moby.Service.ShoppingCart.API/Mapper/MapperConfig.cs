using AutoMapper;
using Moby.Services.ShoppingCart.API.Models;
using Moby.Services.ShoppingCart.API.Models.Dto;

namespace Moby.Services.ShoppingCart.API.Mapper;

public class MapperConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        return new MapperConfiguration(config =>
        {
            config.CreateMap<CartModel, CartDto>().ReverseMap();
            config.CreateMap<CartHeaderModel, CartHeaderDto>().ReverseMap();
            config.CreateMap<CartDetailsModel, CartDetailsDto>().ReverseMap();
            config.CreateMap<ProductModel, ProductDto>().ReverseMap();
        });
    }
}
