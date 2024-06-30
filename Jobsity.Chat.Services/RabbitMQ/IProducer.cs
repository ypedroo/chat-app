namespace Jobsity.Chat.Services.RabbitMQ;

using Domain.Dto;

public interface IProducer
{
    void Send(MessageDto messageDto);
}