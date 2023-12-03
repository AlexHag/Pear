using Microsoft.AspNetCore.SignalR;
using Pear.Discovery.Persistance;
using Pear.Discovery.Entities;

namespace Pear.Discovery.Hubs;

public class MainHub : Hub
{
    private readonly IPearMessageRepository _pearMessageRepository;
    private readonly IPearFriendsRepository _pearFriendsRepository;
    public MainHub(IPearMessageRepository pearMessageRepository, IPearFriendsRepository pearFriendsRepository)
    {
        _pearMessageRepository = pearMessageRepository;
        _pearFriendsRepository = pearFriendsRepository;
    }

    public override async Task OnConnectedAsync()
    {
        var publicKey = Context.GetHttpContext().Request.Headers["publicKey"].ToString();
        Console.WriteLine($"Connecting... ConnectionId: {Context.ConnectionId}\tPublicKey: {publicKey}");

        if (String.IsNullOrWhiteSpace(publicKey))
        {
            await Clients.Caller.SendAsync("Error", "No public key provided.");
            return;
        }
        try
        {
            var pearFriend = await _pearFriendsRepository.GetByPublicKey(publicKey);
            if (pearFriend is null)
            {
                await Clients.Caller.SendAsync("Error", "No pear friend found with that public key.");
                return;
            }
            pearFriend.ConnectionId = Context.ConnectionId;
            pearFriend.IsOnline = true;
            await _pearFriendsRepository.UpdateAsync(pearFriend);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
            return;
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"Disconnecting... ConnectionId: {Context.ConnectionId}");
        var pearFriend = await _pearFriendsRepository.GetByConnectionId(Context.ConnectionId);
        if (pearFriend is null)
        {
            // await Clients.Caller.SendAsync("Error", "No pear friend found with that public key.");
            return;
        }
        pearFriend.ConnectionId = null;
        pearFriend.IsOnline = false;
        await _pearFriendsRepository.UpdateAsync(pearFriend);
        
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string senderPublicKey, string recipientPublicKey, string content, string signature)
    {
        Console.WriteLine($"Sending message... SenderPublicKey: {senderPublicKey}\tRecipientPublicKey: {recipientPublicKey}\tContent: {content}");
        var pearMessage = new PearMessage
        {
            SenderPublicKey = senderPublicKey,
            RecipientPublicKey = recipientPublicKey,
            Content = content,
            SentAt = DateTime.Now,
            Delivered = false
        };

        var recipient = await _pearFriendsRepository.GetByPublicKey(recipientPublicKey);
        if (recipient is null)
        {
            Console.WriteLine("RECIPIENT IS NULL");
            await Clients.Caller.SendAsync("Error", "No pear friend found with that public key.");
            return;
        }

        if (!recipient.IsOnline || String.IsNullOrWhiteSpace(recipient.ConnectionId))
        {
            await _pearMessageRepository.AddAsync(pearMessage);
            return;
        }

        await Clients.Client(recipient.ConnectionId).SendAsync("ReceiveMessage", senderPublicKey, content, signature);
    }
}
