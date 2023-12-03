namespace Pear.Client.Models;

public class SignedMessage
{
    public string PublicKey { get; set; }
    public string Signature { get; set; }
    public string MessageHash { get; set; }
}
