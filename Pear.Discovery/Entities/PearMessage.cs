namespace Pear.Discovery.Entities;

public class PearMessage
{
    public int Id { get; set; }
    public string SenderPublicKey { get; set; }
    public string RecipientPublicKey { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public bool Delivered { get; set; }
}
