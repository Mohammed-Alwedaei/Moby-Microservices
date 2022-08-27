using Moby.Services.Order.API.Models;

namespace Moby.Services.Order.API.Repository;

public interface IOrderManager
{
    Task<bool> AddOrder(OrderHeaderModel orderHeader);

    Task<bool> UpdatePaymentStatus(int orderHeaderId, bool isPaid);
}
