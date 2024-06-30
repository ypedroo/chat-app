using Jobsity.Chat.Repositories.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Jobsity.Chat.Repositories;

public static class Bootstraper
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services
            .AddTransient<IMessageRepository, MessageRepository>();
    }
}