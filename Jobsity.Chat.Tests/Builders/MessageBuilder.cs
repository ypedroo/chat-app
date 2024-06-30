using Jobsity.Chat.Domain.Entities;

namespace Jobsity.Chat.Tests.Builders;

public class MessageBuilder
{
    private readonly Message _instance;

    public MessageBuilder()
    {
        _instance = new Message();
    }
    public Message Build() => _instance;
}