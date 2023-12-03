using Pear.Client.Models;
using System.Text.Json;

namespace Pear.Client.Services;

internal static class KeyManagerService
{
    public static async Task<List<KeyContent>?> GetKeyContents()
    {
        var keys_string = await SecureStorage.Default.GetAsync("keys");

        if (keys_string is not null)
        {
            try
            {
                var keys = JsonSerializer.Deserialize<List<KeyContent>>(keys_string);
                if (keys is not null)
                {
                    return keys;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        return null;
    }

    public static async Task SaveKeys(List<KeyContent> keys)
        => await SecureStorage.Default.SetAsync("keys", JsonSerializer.Serialize(keys));
}
