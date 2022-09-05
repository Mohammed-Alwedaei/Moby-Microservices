using Microsoft.EntityFrameworkCore;
using Moby.Services.Email.API.DbContexts;
using Moby.Services.Email.API.Messages;
using Moby.Services.Email.API.Models;

namespace Moby.Services.Email.API.Repository;

public class EmailManager : IEmailManager
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public EmailManager(DbContextOptions<ApplicationDbContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }


    public async Task SendAndLogEmailAsync(UpdatePaymentProcessResultMessage message)
    {
        //TODO: Implement send grid 


        //Log Email in the database
        var emailLog = new EmailModel
        {
            Email = message.Email,
            Log = $"Order - {message.Id} has been created successfully"
        };

        await using var db = new ApplicationDbContext(_dbContextOptions);

        db.EmailLogs.Add(emailLog);
        await db.SaveChangesAsync();
    }
}
