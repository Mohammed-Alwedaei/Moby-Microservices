using AutoMapper;
using Moby.Service.ShoppingCart.API.Models;
using Moby.Service.ShoppingCart.API.Models.Dto;

namespace Moby.Service.ShoppingCart.API.Mapper;

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
