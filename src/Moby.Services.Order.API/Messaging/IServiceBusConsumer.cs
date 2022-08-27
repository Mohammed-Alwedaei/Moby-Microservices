namespace Moby.Services.Order.API.Messaging;

public interface IServiceBusConsumer
{
    public Task Start();

    public Task Stop();
}
