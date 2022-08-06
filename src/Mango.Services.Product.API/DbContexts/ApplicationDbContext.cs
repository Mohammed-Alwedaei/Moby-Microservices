using Microsoft.EntityFrameworkCore;
using Moby.Services.Product.API.Models;

namespace Moby.Services.Product.API.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ProductModel> Products { get; set; }
    }
}
