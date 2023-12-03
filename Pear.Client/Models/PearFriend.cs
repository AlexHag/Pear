using SQLite;

namespace Pear.Client.Models;

[Table("PearFriends")]
public class PearFriend
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string PublicKey { get; set; }
}
