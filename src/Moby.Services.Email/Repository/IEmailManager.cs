using Moby.Services.Email.API.Messages;

namespace Moby.Services.Email.API.Repository;

public interface IEmailManager
{
    Task SendAndLogEmailAsync(UpdatePaymentProcessResultMessage message);
}
