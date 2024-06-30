namespace Jobsity.Chat.Repositories.Messages;

using Domain.Entities;

public interface IMessageRepository
{
    Task AddAsync(Message message);
    Task<IEnumerable<Message>> GetHistoryMessagesAsync();
}