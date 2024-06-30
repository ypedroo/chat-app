using Jobsity.Chat.Domain.Configuration;
using Jobsity.Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Jobsity.Chat.Repositories.Messages;

public class MessageRepository(ApplicationConfig applicationConfig) : IMessageRepository
{
    public async Task AddAsync(Message message)
    {
        await using var ctx = new ApplicationDbContext();
        await ctx.Messages.AddAsync(message);
        await ctx.SaveChangesAsync();
    }

    public async Task<IEnumerable<Message>> GetHistoryMessagesAsync()
    {
        await using var ctx = new ApplicationDbContext();
        return await ctx.Messages
            .OrderBy(message => message.CreationDate)
            .Take(applicationConfig.MessagesShown)
            .ToListAsync();
    }
}