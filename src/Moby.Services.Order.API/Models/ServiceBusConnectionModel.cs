namespace Moby.Services.Order.API.Models;

public class ServiceBusConnectionModel
{
    public string? ConnectionString { get; set; }

    public string? TopicName { get; set; }

    public string SubscriptionName { get; set; }
}
