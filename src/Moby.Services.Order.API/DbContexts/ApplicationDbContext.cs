using Microsoft.EntityFrameworkCore;
using Moby.Services.Order.API.Models;

namespace Moby.Services.Order.API.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<OrderDetailsModel> Details { get; set; }

    public DbSet<OrderHeaderModel> Headers { get; set; }
}
