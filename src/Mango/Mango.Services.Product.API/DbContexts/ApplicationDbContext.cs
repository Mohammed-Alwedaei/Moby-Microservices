using Mango.Services.Product.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Product.API.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<ProductModel> Products { get; set; }
    }
}
