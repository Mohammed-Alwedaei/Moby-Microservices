namespace Moby.Services.Email.API.Messaging;

public interface IServiceBusConsumer
{
    public Task Start();

    public Task Stop();
}
