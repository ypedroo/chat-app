using FluentValidation;
using Jobsity.Chat.Domain.Configuration;

namespace Jobsity.Chat.Domain.Validators;

public class ApplicationConfigValidator : AbstractValidator<ApplicationConfig>
{
    public ApplicationConfigValidator()
    {
        RuleFor(config => config.ConnectionString).NotEmpty()
            .WithMessage(Constants.ErrorMessages.MissingApplicationConfigError);

        When(config => config.StockApi is not null, () =>
        {
            RuleFor(config => config.StockApi!.BaseUrl).NotEmpty()
                .WithMessage(Constants.ErrorMessages.MissingApplicationConfigError);
            RuleFor(config => config.StockApi!.GetStockEndpoint).NotEmpty()
                .WithMessage(Constants.ErrorMessages.MissingApplicationConfigError);
        });

        When(config => config.RabbitMq is not null, () =>
        {
            RuleFor(config => config.RabbitMq!.Hostname).NotEmpty()
                .WithMessage(Constants.ErrorMessages.MissingApplicationConfigError);
        });
    }
}