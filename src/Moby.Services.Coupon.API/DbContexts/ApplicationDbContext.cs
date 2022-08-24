using Microsoft.EntityFrameworkCore;
using Moby.Services.Coupon.API.Models;

namespace Moby.Services.Coupon.API.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<CouponModel> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CouponModel>().HasData(new CouponModel
        {
            Id = 1,
            Code = "10OFF",
            DiscountAmount = 10m
        });

        modelBuilder.Entity<CouponModel>().HasData(new CouponModel
        {
            Id = 2,
            Code = "20OFF",
            DiscountAmount = 20m
        });
    }
}
