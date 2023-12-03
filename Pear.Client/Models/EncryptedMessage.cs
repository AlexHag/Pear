namespace Pear.Client.Models;

public class EncryptedMessage
{
    public string SenderPublicKey { get; set; }
    public string RecipientPublicKey { get; set; }
    public string Cipher { get; set; }
    public string Signature { get; set; }
}
