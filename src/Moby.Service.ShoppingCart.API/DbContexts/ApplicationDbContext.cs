using Microsoft.EntityFrameworkCore;
using Moby.Service.ShoppingCart.API.Models;
using Moby.Service.ShoppingCart.API.Models.Dto;

namespace Moby.Service.ShoppingCart.API.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    {
        
    }

    public DbSet<CartHeaderModel> CartHeaders { get; set; }

    public DbSet<CartDetailsModel> CartDetails { get; set; }

    public DbSet<ProductModel> Products { get; set; }
}