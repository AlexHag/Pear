using Pear.Discovery.Entities;
using Microsoft.EntityFrameworkCore;
using Pear.Discovery.Exceptions;

namespace Pear.Discovery.Persistance;

public interface IPearFriendsRepository
{
    Task AddAsync(PearFriend pearFriend);
    Task UpdateAsync(PearFriend pearFriend);
    Task<PearFriend?> GetByName(string name);
    Task<PearFriend?> GetByPublicKey(string publicKey);
    Task<List<PearFriend>> GetAllAsync();
    Task<PearFriend> GetByConnectionId(string connectionId);
    Task UpdateConnectionId(string publicKey, string connectionId);
}

public class PearFriendsRepository : IPearFriendsRepository
{
    private readonly PearDbContext _dbContext;
    public PearFriendsRepository(PearDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(PearFriend pearFriend)
    {
        var existingPear = await _dbContext.PearFriends.SingleOrDefaultAsync(p => p.PublicKey == pearFriend.PublicKey);
        if (existingPear is not null)
            throw new PublicKeyAlreadyExistsException($"A pear with the public key {pearFriend.PublicKey} already exists.");

        await _dbContext.PearFriends.AddAsync(pearFriend);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(PearFriend pearFriend)
    {
        _dbContext.PearFriends.Update(pearFriend);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<PearFriend?> GetByName(string name)
    {
        var pearFriend = await _dbContext.PearFriends.SingleOrDefaultAsync(p => p.Name == name);
        return pearFriend;
    }

    public async Task<PearFriend?> GetByPublicKey(string publicKey)
    {
        var pearFriend = await _dbContext.PearFriends.SingleOrDefaultAsync(p => p.PublicKey == publicKey);
        return pearFriend;
    }

    public async Task<List<PearFriend>> GetAllAsync()
    {
        var pearFriends = await _dbContext.PearFriends.ToListAsync();
        return pearFriends;
    }

    public async Task<PearFriend> GetByConnectionId(string connectionId)
    {
        var pearFriend = await _dbContext.PearFriends.SingleOrDefaultAsync(p => p.ConnectionId == connectionId);
        return pearFriend;
    }

    public async Task UpdateConnectionId(string publicKey, string connectionId)
    {
        var pearFriend = await _dbContext.PearFriends.SingleOrDefaultAsync(p => p.PublicKey == publicKey);
        if (pearFriend is null)
            throw new PearNotFoundException($"A pear with the public key {publicKey} was not found.");

        pearFriend.ConnectionId = connectionId;
        await _dbContext.SaveChangesAsync();
    }
}