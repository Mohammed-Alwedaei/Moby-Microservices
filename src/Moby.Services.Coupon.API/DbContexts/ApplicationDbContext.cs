using Microsoft.EntityFrameworkCore;
using Moby.Services.Coupon.API.Models;

namespace Moby.Services.Coupon.API.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<CouponModel> Coupons { get; set; }
}
