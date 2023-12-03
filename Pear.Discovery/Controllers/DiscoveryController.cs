using Microsoft.AspNetCore.Mvc;
using Pear.Discovery.Persistance;
using Pear.Discovery.Entities;
using Pear.Discovery.Exceptions;

namespace Pear.Discovery.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscoveryController : ControllerBase
{
    private readonly IPearFriendsRepository _pearFriendRepo;

    public DiscoveryController(IPearFriendsRepository pearFriendRepo)
    {
        _pearFriendRepo = pearFriendRepo;
    }

    [HttpPost]
    public async Task<IActionResult> Index(PearFriend pearFriend)
    {
        try
        {
            await _pearFriendRepo.AddAsync(pearFriend);
        }
        catch (PublicKeyAlreadyExistsException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        return Ok();
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var pearFriend = await _pearFriendRepo.GetByName(name);
        if (pearFriend is null)
            return NotFound();
        return Ok(pearFriend);
    }
}
