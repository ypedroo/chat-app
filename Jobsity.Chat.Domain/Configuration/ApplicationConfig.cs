using Jobsity.Chat.Domain.Exceptions;
using Jobsity.Chat.Domain.Validators;
using Serilog;

namespace Jobsity.Chat.Domain.Configuration;

public class ApplicationConfig
{
    public int MessagesShown { get; set; }
    public string? ConnectionString { get; set; }
    public RabbitMq? RabbitMq { get; set; }
    public StockApi? StockApi { get; set; }

    public void Validate()
    {
        var validationResult = new ApplicationConfigValidator().Validate(this);
        if (validationResult.IsValid) return;

        var errors = validationResult.Errors.Select(c => c.ErrorMessage).ToList();

        Log.Error("Configuration: Contains errors: {@Errors}", errors);
        throw new ErrorConfigurationException(string.Join(",", errors));
    }
}