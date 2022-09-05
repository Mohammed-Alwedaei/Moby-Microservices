using Microsoft.EntityFrameworkCore;
using Moby.Services.Email.API.Models;

namespace Moby.Services.Email.API.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<EmailModel> EmailLogs { get; set; }
}
