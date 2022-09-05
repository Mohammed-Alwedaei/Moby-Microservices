namespace Moby.Services.Email.API.Models;

public class ServiceBusConnectionModel
{
    public string? ConnectionString { get; set; }

    public string? PaymentConsumerTopic { get; set; }

    public string? EmailSubscription { get; set; }
}
