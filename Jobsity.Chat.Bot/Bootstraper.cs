using Microsoft.Extensions.DependencyInjection;
using Jobsity.Chat.Domain.Configuration;

namespace Jobsity.Chat.Bot;

public static class Bootstraper
{
    public static void AddBotProcessor(this IServiceCollection services, ApplicationConfig applicationConfig)
    {
        services.AddTransient<IBotProcessor, BotProcessor>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new BotProcessor(factory, applicationConfig.StockApi!.GetStockEndpoint!);
        });
    }
}