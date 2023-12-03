namespace Pear.Discovery.Entities;

public class PearFriend
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PublicKey { get; set; }
    public string? ConnectionId { get; set; }
    public bool IsOnline { get; set; }
}
