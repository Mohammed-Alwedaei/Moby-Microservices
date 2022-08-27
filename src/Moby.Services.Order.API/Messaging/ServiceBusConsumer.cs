using Azure.Messaging.ServiceBus;
using System.Text;
using Newtonsoft.Json;
using Moby.Services.Order.API.Messages;
using Moby.Services.Order.API.Models;
using Moby.Services.Order.API.Repository;
using System.Diagnostics;
using Moby.ServiceBus;

namespace Moby.Services.Order.API.Messaging;

public class ServiceBusConsumer : IServiceBusConsumer
{
    private readonly OrderManager _orderManager;

    private readonly IConfiguration _configuration;

    private readonly ServiceBusConnectionModel _serviceBusConnection;

    private readonly ServiceBusProcessor _serviceBusProcessor;

    private readonly ServiceBusProcessor _paymentServiceBusProcessor;

    private readonly IMessageBusManager _messageBusManager;

    public ServiceBusConsumer(OrderManager orderManager, IConfiguration configuration, IMessageBusManager messageBusManager)
    {
        _orderManager = orderManager;

        _configuration = configuration;

        _messageBusManager = messageBusManager;

        _serviceBusConnection = new ServiceBusConnectionModel
        {
            ConnectionString = _configuration.GetValue<string>("ServiceBus:ConnectionString"),
            CheckoutTopic = _configuration.GetValue<string>("ServiceBus:CheckoutTopic"),
            CheckoutSubscription = _configuration.GetValue<string>("ServiceBus:CheckoutSubscription"),
            PaymentProcessTopic = _configuration.GetValue<string>("ServiceBus:PaymentProcessTopic"),
            PaymentProcessSubscription = _configuration.GetValue<string>("ServiceBus:PaymentProcessSubscription"),
            PaymentUpdateTopic = _configuration.GetValue<string>("ServiceBus:PaymentConsumerTopic"),
            PaymentUpdateSubscription = _configuration.GetValue<string>("ServiceBus:PaymentConsumerSubscription"),
        };

        var client = new ServiceBusClient(_serviceBusConnection.ConnectionString);

        _serviceBusProcessor = client
            .CreateProcessor(_serviceBusConnection.CheckoutTopic, _serviceBusConnection.CheckoutSubscription);

        _paymentServiceBusProcessor = client
            .CreateProcessor(_serviceBusConnection.PaymentUpdateTopic, _serviceBusConnection.PaymentUpdateSubscription);
    }

    public async Task Start()
    {
        _serviceBusProcessor.ProcessMessageAsync += OnMessageReceivedEvent;
        _serviceBusProcessor.ProcessErrorAsync += OnErrorReceivedEvent;

        _paymentServiceBusProcessor.ProcessMessageAsync += OnPaymentProcessEvent;
        _paymentServiceBusProcessor.ProcessErrorAsync += OnErrorReceivedEvent;

        await _serviceBusProcessor.StartProcessingAsync();
        await _paymentServiceBusProcessor.StartProcessingAsync();
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

        CheckoutMessage checkOutMessage = JsonConvert.DeserializeObject<CheckoutMessage>(body);

        var orderHeader = new OrderHeaderModel
        {
            UserId = checkOutMessage.UserId,
            FirstName = checkOutMessage.FirstName,
            LastName = checkOutMessage.LastName,
            Details = new List<OrderDetailsModel>(),
            CardNumber = checkOutMessage.CardNumber,
            CouponCode = checkOutMessage.CouponCode,
            Cvv = checkOutMessage.Cvv,
            TotalAfterDiscount = checkOutMessage.TotalAfterDiscount,
            Total = checkOutMessage.Total,
            Email = checkOutMessage.Email,
            ItemsCount = 0,
            ExpiryDate = checkOutMessage.ExpiryDate,
            PickUpDate = checkOutMessage.PickUpDate,
            OrderTime = DateTime.Now,
            PaymentStatus = false,
            PhoneNumber = checkOutMessage.PhoneNumber,
        };

        foreach (var detail in checkOutMessage.Details)
        {
            var orderDetail = new OrderDetailsModel
            {
                CartHeaderId = detail.CartHeaderId,
                ProductId = detail.ProductId,
                ProductPrice = detail.Product.Price,
                ProductName = detail.Product.Name,
                Count = detail.Count
            };

            orderHeader.ItemsCount += detail.Count;
            orderHeader.Details.Add(orderDetail);
        }

        await _orderManager.AddOrder(orderHeader);

        var paymentProcessRequest = new PaymentProcessRequestMessage
        {
            OrderId = orderHeader.Id,
            Name = $"{orderHeader.FirstName} {orderHeader.LastName}",
            CardNumber = orderHeader.CardNumber,
            Cvv = orderHeader.Cvv,
            ExpiryDate = orderHeader.ExpiryDate,
            OrderTotal = orderHeader.Total,
        };

        try
        {
            await _messageBusManager.PublishMessage(
                paymentProcessRequest,
                _serviceBusConnection.PaymentProcessTopic,
                _serviceBusConnection.ConnectionString);

            await args.CompleteMessageAsync(args.Message);

        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }

    private async Task OnPaymentProcessEvent(ProcessMessageEventArgs args)
    {
        var message = args.Message;

        var body = Encoding.UTF8.GetString(message.Body);

        var updateRequestMessage = JsonConvert.DeserializeObject<UpdatePaymentProcessResultMessage>(body);

        await _orderManager.UpdatePaymentStatus(updateRequestMessage.Id, updateRequestMessage.Status);

        await args.CompleteMessageAsync(args.Message);
    }

    private Task OnErrorReceivedEvent(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.Message);
        return Task.CompletedTask;
    }
}
