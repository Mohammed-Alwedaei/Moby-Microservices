namespace Moby.Services.Payments.API.Messaging;

public interface IServiceBusConsumer
{
    public Task Start();

    public Task Stop();
}
