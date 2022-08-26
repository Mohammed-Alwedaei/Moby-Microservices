using Azure.Messaging.ServiceBus;
using System.Text;
using Newtonsoft.Json;

namespace Moby.ServiceBus;

public class MessageBusManager : IMessageBusManager
{
    public async Task PublishMessage(BaseMessageModel baseMessage, string topicName, string connectionString)
    {
        await using var client = new ServiceBusClient(connectionString);

        var sender = client.CreateSender(topicName);

        var message = JsonConvert.SerializeObject(baseMessage);

        var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(message))
        {
            CorrelationId = Guid.NewGuid().ToString()
        };

        await sender.SendMessageAsync(finalMessage);

        await sender.DisposeAsync();
    }
}
