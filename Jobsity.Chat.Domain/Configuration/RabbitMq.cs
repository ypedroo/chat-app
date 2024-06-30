namespace Jobsity.Chat.Domain.Configuration;

public class RabbitMq
{
    public string? Hostname { get; set; }
    public string QueueName { get; set; }
}