using Pear.Discovery.Entities;
using Microsoft.EntityFrameworkCore;

namespace Pear.Discovery.Persistance;

public interface IPearMessageRepository
{
    Task AddAsync(PearMessage pearMessage);
    Task GetConversation(string senderPublicKey, string recipientPublicKey);
    Task DeleteMessage(int id);
}

public class PearMessageRepository : IPearMessageRepository
{
    private readonly PearDbContext _dbContext;
    public PearMessageRepository(PearDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(PearMessage pearMessage)
    {
        await _dbContext.PearMessages.AddAsync(pearMessage);
        await _dbContext.SaveChangesAsync();
    }

    public async Task GetConversation(string senderPublicKey, string recipientPublicKey)
    {
        var pearMessages = await _dbContext.PearMessages
            .Where(p => p.SenderPublicKey == senderPublicKey && p.RecipientPublicKey == recipientPublicKey)
            .ToListAsync();
    }

    public async Task DeleteMessage(int id)
    {
        var pearMessage = await _dbContext.PearMessages.FindAsync(id);
        _dbContext.PearMessages.Remove(pearMessage);
        await _dbContext.SaveChangesAsync();
    }
}