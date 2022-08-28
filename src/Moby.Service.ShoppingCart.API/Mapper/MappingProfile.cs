using AutoMapper;
using Moby.Services.ShoppingCart.API.Models;
using Moby.Services.ShoppingCart.API.Models.Dto;

namespace Moby.Services.ShoppingCart.API.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CartModel, CartDto>().ReverseMap();
        CreateMap<CartHeaderModel, CartHeaderDto>().ReverseMap();
        CreateMap<CartDetailsModel, CartDetailsDto>().ReverseMap();
        CreateMap<ProductModel, ProductDto>().ReverseMap();
    }
}
