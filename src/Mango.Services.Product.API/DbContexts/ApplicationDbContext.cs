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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductModel>().HasData(new ProductModel
            {
                Id = 1,
                Name = "Samosa",
                Price = 15,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://mobillia.blob.core.windows.net/mobillia/11.jpg",
                Category = "Appetizer"
            });

            modelBuilder.Entity<ProductModel>().HasData(new ProductModel
            {
                Id = 2,
                Name = "Paneer Tikka",
                Price = 13.99m,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://mobillia.blob.core.windows.net/mobillia/12.jpg",
                Category = "Appetizer"
            });

            modelBuilder.Entity<ProductModel>().HasData(new ProductModel
            {
                Id = 3,
                Name = "Sweet Pie",
                Price = 10.99m,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://mobillia.blob.core.windows.net/mobillia/13.jpg",
                Category = "Dessert"
            });

            modelBuilder.Entity<ProductModel>().HasData(new ProductModel
            {
                Id = 4,
                Name = "Pav Bhaji",
                Price = 15m,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://mobillia.blob.core.windows.net/mobillia/14.jpg",
                Category = "Entree"
            });
        }
    }
}
