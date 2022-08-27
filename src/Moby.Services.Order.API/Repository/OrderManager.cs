using Microsoft.EntityFrameworkCore;
using Moby.Services.Order.API.DbContexts;
using Moby.Services.Order.API.Models;

namespace Moby.Services.Order.API.Repository;

public class OrderManager : IOrderManager
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public OrderManager(DbContextOptions<ApplicationDbContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }

    public async Task<bool> AddOrder(OrderHeaderModel orderHeader)
    {
        await using var db = new ApplicationDbContext(_dbContextOptions);
        db.Headers.Add(orderHeader);

        var isSuccess = await db.SaveChangesAsync();

        return isSuccess == 1 ? true : false;
    }

    public async Task<bool> UpdatePaymentStatus(int orderHeaderId, bool isPaid)
    {
        await using var db = new ApplicationDbContext(_dbContextOptions);

        var orderHeaderFromDb = await db.Headers
            .FirstOrDefaultAsync(x => x.Id == orderHeaderId);

        if (orderHeaderFromDb is null)
            return false;

        orderHeaderFromDb.PaymentStatus = isPaid;

        db.Headers.Update(orderHeaderFromDb);

        await db.SaveChangesAsync();

        return true;
    }
}
