using AutoMapper;
using Moby.Services.Coupon.API.Models;
using Moby.Services.Coupon.API.Models.Dtos;

namespace Moby.Services.Coupon.API.Mapper;

public class MapperConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        return new MapperConfiguration(config =>
        {
            config.CreateMap<CouponModel, CouponDto>().ReverseMap();
        });
    }
}
