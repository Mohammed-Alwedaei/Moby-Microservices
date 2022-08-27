using Microsoft.EntityFrameworkCore;
using Moby.Services.ShoppingCart.API.Models;

namespace Moby.Services.ShoppingCart.API.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<CartHeaderModel> CartHeaders { get; set; }

    public DbSet<CartDetailsModel> CartDetails { get; set; }

    public DbSet<ProductModel> Products { get; set; }
}