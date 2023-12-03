using SQLite;

namespace Pear.Client.Models;

[Table("PearMessages")]
public class PearMessage
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string SenderPublicKey { get; set; }
    public string RecipientPublicKey { get; set; }
    public string Content { get; set; }
}
