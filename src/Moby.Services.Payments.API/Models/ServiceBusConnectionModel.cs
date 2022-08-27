namespace Moby.Services.Payments.API.Models;

public class ServiceBusConnectionModel
{
    public string? ConnectionString { get; set; }

    public string? PaymentProcessTopic { get; set; }

    public string? PaymentProcessSubscription { get; set; }

    public string? PaymentConsumerTopic { get; set; }

    public string? PaymentConsumerSubscription { get; set; }
}
