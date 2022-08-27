using Azure.Messaging.ServiceBus;
using System.Text;
using Newtonsoft.Json;
using Moby.Services.Order.API.Messages;
using Moby.Services.Order.API.Models;
using Moby.Services.Order.API.Repository;
using System.Diagnostics;

namespace Moby.Services.Order.API.Messaging;

public class ServiceBusConsumer : IServiceBusConsumer
{
    private readonly OrderManager _orderManager;

    private readonly IConfiguration _configuration;

    private readonly ServiceBusConnectionModel _serviceBusConnection;

    private readonly ServiceBusProcessor _serviceBusProcessor;

    public ServiceBusConsumer(OrderManager orderManager, IConfiguration configuration)
    {
        _orderManager = orderManager;

        _configuration = configuration;

        _serviceBusConnection = new ServiceBusConnectionModel
        {
            ConnectionString = _configuration
                .GetValue<string>("ServiceBus:ConnectionString"),
            TopicName = _configuration
                .GetValue<string>("ServiceBus:TopicName"),
            SubscriptionName = _configuration
                .GetValue<string>("ServiceBus:SubscriptionName")
        };

        var client = new ServiceBusClient(_serviceBusConnection.ConnectionString);

        _serviceBusProcessor = client
            .CreateProcessor(_serviceBusConnection.TopicName, _serviceBusConnection.SubscriptionName);
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
    }

    private Task OnErrorReceivedEvent(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.Message);
        return Task.CompletedTask;
    }
}
