using Jobsity.Chat.Domain;
using Jobsity.Chat.Domain.Configuration;
using Jobsity.Chat.Services.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Jobsity.Chat.Services;

public static class Bootstraper
{
    public static void AddServices(this IServiceCollection services)
    {
        services
            .AddTransient<IConsumer, Consumer>()
            .AddTransient<IProducer, Producer>()
            .AddSignalR();
    }

    public static void AddHttpClients(this IServiceCollection services, ApplicationConfig applicationConfig)
    {
        services.AddHttpClient(Constants.StockApiClientName, c =>
        {
            c.BaseAddress = new Uri(applicationConfig.StockApi!.BaseUrl!);
            c.DefaultRequestHeaders.Add("Accept", "application/json");
        }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip
        });
    }
}