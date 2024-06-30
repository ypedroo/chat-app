using AutoMapper;
using Jobsity.Chat.Domain.Mappers;
using Jobsity.Chat.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Jobsity.Chat.Domain;

public static class Bootstraper
{
    public static void AddValidators(this IServiceCollection services)
    {
        services
            .AddScoped<ApplicationConfigValidator>();
    }

    public static void AddMapperProfile(this IServiceCollection services)
    {
        services.AddSingleton(_ =>
            new MapperConfiguration(cfg => { cfg.AddProfile(new MapperProfile()); }).CreateMapper());
    }
}