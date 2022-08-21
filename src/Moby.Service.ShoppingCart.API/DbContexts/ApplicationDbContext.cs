using Microsoft.EntityFrameworkCore;

namespace Moby.Service.ShoppingCart.API.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    {
        
    }


}