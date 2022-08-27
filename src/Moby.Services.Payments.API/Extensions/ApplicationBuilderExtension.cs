using Microsoft.Extensions.DependencyInjection;
using Moby.Services.Payments.API.Messaging;

namespace Moby.Services.Payments.API.Extensions;

public static class ApplicationBuilderExtension
{
    public static IServiceBusConsumer ServiceBusConsumer { get; set; }

    public static IApplicationBuilder UseServiceBusConsumer(this IApplicationBuilder applicationBuilder)
    {
        ServiceBusConsumer = applicationBuilder.ApplicationServices.GetService<IServiceBusConsumer>();

        var hostApplicationLifetime = applicationBuilder.ApplicationServices.GetService<IHostApplicationLifetime>();

        hostApplicationLifetime.ApplicationStarted.Register(OnStart);
        hostApplicationLifetime.ApplicationStopped.Register(OnStop);

        return applicationBuilder;
    }

    private static void OnStart()
    {
        ServiceBusConsumer.Start();
    }

    private static void OnStop()
    {
        ServiceBusConsumer.Stop();
    }
}
