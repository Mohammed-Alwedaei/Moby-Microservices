using Moby.ServiceBus;

namespace Moby.Services.Payments.API.Messages;

public class UpdatePaymentProcessResultMessage : BaseMessageModel
{
    public int Id { get; set; }

    public bool Status { get; set; }

    public string? Email { get; set; }
}
