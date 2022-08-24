using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moby.Services.Coupon.API.DbContexts;
using Moby.Services.Coupon.API.Models;
using Moby.Services.Coupon.API.Models.Dtos;

namespace Moby.Services.Coupon.API.Repository;

public class CouponManager : ICouponManager
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CouponManager(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<CouponDto> GetCouponByCodeAsync(string code)
    {
        var couponFromDb = await _db.Coupons.FirstOrDefaultAsync(c => c.Code == code);

        return _mapper.Map<CouponModel, CouponDto>(couponFromDb);
    }
}
