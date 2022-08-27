namespace Moby.Services.Order.API.Models;

public class ServiceBusConnectionModel
{
    public string? ConnectionString { get; set; }

    public string? CheckoutTopic { get; set; }

    public string? CheckoutSubscription { get; set; }

    public string? PaymentProcessTopic { get; set; }

    public string? PaymentProcessSubscription { get; set; }

    public string? PaymentUpdateTopic { get; set; }

    public string? PaymentUpdateSubscription { get; set; }
}
