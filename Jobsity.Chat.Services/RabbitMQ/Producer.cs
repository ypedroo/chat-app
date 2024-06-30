using RabbitMQ.Client;
using Jobsity.Chat.Domain.Configuration;
using Jobsity.Chat.Domain.Dto;
using Newtonsoft.Json;
using System.Text;

namespace Jobsity.Chat.Services.RabbitMQ;

public class Producer(ApplicationConfig applicationConfig) : IProducer
{
    private readonly RabbitMq _rabbitMq = applicationConfig.RabbitMq!;

    public void Send(MessageDto messageDto)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMq.Hostname,
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: _rabbitMq.QueueName, durable: true, exclusive: false, autoDelete: false);

        var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageDto));
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(string.Empty, _rabbitMq.QueueName, true, properties, sendBytes);
    }
}