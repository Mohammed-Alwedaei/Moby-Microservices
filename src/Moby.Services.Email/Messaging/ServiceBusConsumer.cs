using Azure.Messaging.ServiceBus;
using System.Text;
using System.Diagnostics;
using Moby.Services.Email.API.Messages;
using Moby.Services.Email.API.Models;
using Moby.Services.Email.API.Repository;
using Newtonsoft.Json;

namespace Moby.Services.Email.API.Messaging;

public class ServiceBusConsumer : IServiceBusConsumer
{
    private readonly EmailManager _emailManager;

    private readonly IConfiguration _configuration;

    private readonly ServiceBusConnectionModel _serviceBusConnection;

    private readonly ServiceBusProcessor _serviceBusProcessor;

    private readonly ServiceBusProcessor _paymentServiceBusProcessor;

    public ServiceBusConsumer(EmailManager emailManager, IConfiguration configuration)
    {
        _emailManager = emailManager;
        _configuration = configuration;

        _serviceBusConnection = new ServiceBusConnectionModel
        {
            ConnectionString = _configuration.GetValue<string>("ServiceBus:ConnectionString"),
            PaymentConsumerTopic = _configuration.GetValue<string>("ServiceBus:TopicName"),
            EmailSubscription = _configuration.GetValue<string>("ServiceBus:SubscriptionName"),
        };

        var client = new ServiceBusClient(_serviceBusConnection.ConnectionString);

        _serviceBusProcessor = client
            .CreateProcessor(_serviceBusConnection.PaymentConsumerTopic, _serviceBusConnection.EmailSubscription);
    }

    public async Task Start()
    {
        _serviceBusProcessor.ProcessMessageAsync += OnMessageReceivedEvent;
        _serviceBusProcessor.ProcessErrorAsync += OnErrorReceivedEvent;

        await _serviceBusProcessor.StartProcessingAsync();
    }

    public async Task Stop()
    {
        await _serviceBusProcessor.StopProcessingAsync();
        await _serviceBusProcessor.DisposeAsync();
    }

    private async Task OnMessageReceivedEvent(ProcessMessageEventArgs args)
    {
        var message = args.Message;

        var body = Encoding.UTF8.GetString(message.Body);

        UpdatePaymentProcessResultMessage paymentUpdateMessage = JsonConvert.DeserializeObject<UpdatePaymentProcessResultMessage>(body);

        try
        {
            await _emailManager.SendAndLogEmailAsync(paymentUpdateMessage);

            await args.CompleteMessageAsync(args.Message);

        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }

    private Task OnErrorReceivedEvent(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.Message);
        return Task.CompletedTask;
    }
}
