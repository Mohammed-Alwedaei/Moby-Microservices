using Moby.ServiceBus;

namespace Moby.Services.Payments.API.Messages;

public class PaymentProcessRequestMessage : BaseMessageModel
{
    public int OrderId { get; set; }

    public string Name { get; set; }

    public string CardNumber { get; set; }

    public string Cvv { get; set; }

    public string ExpiryDate { get; set; }

    public decimal OrderTotal { get; set; }

    public string Email { get; set; }
}
