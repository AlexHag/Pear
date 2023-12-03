using System.Text.Json.Serialization;

namespace Pear.Client.Models;

public class KeyContent
{
    public required string Name { get; set; }
    public KeyPair KeyPair { get; set; }
    public bool IsDiscovered { get; set; }
    [JsonIgnore]
    public bool IsInEditMode { get; set; }
}