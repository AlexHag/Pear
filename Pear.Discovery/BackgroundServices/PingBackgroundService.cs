// using Pear.Discovery.Persistance;
// using Pear.Discovery.Hubs;
// using Microsoft.AspNetCore.SignalR;

// namespace Pear.Discovery.BackgroundServices;

// public class PingBackgroundService : BackgroundService
// {
//     private readonly ILogger<PingBackgroundService> _logger;
//     private readonly IPearFriendsRepository _pearFriendRepository;
//     private readonly IHubContext<MainHub> _hubContext;

//     public PingBackgroundService(ILogger<PingBackgroundService> logger, IPearFriendsRepository pearFriendRepository, IHubContext<MainHub> hubContext)
//     {
//         _logger = logger;
//         _pearFriendRepository = pearFriendRepository;
//         _hubContext = hubContext;
//     }

//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         while (!stoppingToken.IsCancellationRequested)
//         {
//             _logger.LogInformation("PingBackgroundService running at: {time}", DateTimeOffset.Now);

//             var pearFriends = await _pearFriendRepository.GetAllAsync();

//             foreach (var pearFriend in pearFriends)
//             {
//                 if (pearFriend.IsOnline)
//                 {
//                     try
//                     {
//                         await _hubContext.Clients.Client(pearFriend.ConnectionId).SendAsync("Ping");
//                     }
//                     catch (Exception ex)
//                     {
//                         _logger.LogError(ex, "Error pinging pear friend {name}", pearFriend.Name);
//                         pearFriend.IsOnline = false;
//                         pearFriend.ConnectionId = null;
//                         await _pearFriendRepository.UpdateAsync(pearFriend);
//                     }
//                 }
//             }

//             await Task.Delay(10000, stoppingToken);
//         }
//     }

// }