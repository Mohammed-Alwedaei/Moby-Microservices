using Azure.Messaging.ServiceBus;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;
using Moby.Payments;
using Moby.ServiceBus;
using Moby.Services.Payments.API.Models;
using Moby.Services.Payments.API.Messages;

namespace Moby.Services.Payments.API.Messaging;

public class ServiceBusConsumer : IServiceBusConsumer
{

    private readonly IPaymentProcessor _paymentProcessor;

    private readonly IConfiguration _configuration;

    private readonly ServiceBusConnectionModel _serviceBusConnection;

    private readonly ServiceBusProcessor _serviceBusProcessor;

    private readonly IMessageBusManager _messageBusManager;

    public ServiceBusConsumer(IPaymentProcessor paymentProcessor, IConfiguration configuration, IMessageBusManager messageBusManager)
    {
        _paymentProcessor = paymentProcessor;
        _configuration = configuration;

        _messageBusManager = messageBusManager;

        _serviceBusConnection = new ServiceBusConnectionModel
        {
            ConnectionString = _configuration.GetValue<string>("ServiceBus:ConnectionString"),
            PaymentProcessTopic = _configuration.GetValue<string>("ServiceBus:PaymentProcessTopic"),
            PaymentProcessSubscription = _configuration.GetValue<string>("ServiceBus:PaymentProcessSubscription"),
            PaymentConsumerTopic = _configuration.GetValue<string>("ServiceBus:PaymentConsumerTopic"),
            PaymentConsumerSubscription = _configuration.GetValue<string>("ServiceBus:PaymentConsumerSubscription")
        };

        var client = new ServiceBusClient(_serviceBusConnection.ConnectionString);

        _serviceBusProcessor = client
            .CreateProcessor(_serviceBusConnection.PaymentProcessTopic, _serviceBusConnection.PaymentProcessSubscription);
    }

    public async Task Start()
    {
        _serviceBusProcessor.ProcessMessageAsync += OnPaymentProcess;
        _serviceBusProcessor.ProcessErrorAsync += OnErrorReceivedEvent;
        await _serviceBusProcessor.StartProcessingAsync();
    }

    public async Task Stop()
    {
        await _serviceBusProcessor.StopProcessingAsync();
        await _serviceBusProcessor.DisposeAsync();
    }

    private async Task OnPaymentProcess(ProcessMessageEventArgs args)
    {
        var message = args.Message;

        var body = Encoding.UTF8.GetString(message.Body);

        var paymentProcessRequest = JsonConvert.DeserializeObject<PaymentProcessRequestMessage>(body);

        var paymentProcessStatus = _paymentProcessor.ProcessPayment();

        var updatePaymentProcessResultMessage = new UpdatePaymentProcessResultMessage()
        {
            Id = paymentProcessRequest.OrderId,
            Status = paymentProcessStatus
        };

        try
        {
            await _messageBusManager.PublishMessage(
                updatePaymentProcessResultMessage,
                _serviceBusConnection.PaymentConsumerTopic,
                _serviceBusConnection.ConnectionString);

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
