using MessagePack;

namespace MagicOnionSample.ClientShared.Models
{
    [MessagePackObject]
    public class AuthenticateResponse
    {
        [Key(0)] public string AccessToken { get; set; }
        [Key(1)] public string RefreshToken { get; set; }
        [Key(2)] public long ExpiresIn { get; set; }
        [Key(3)] public bool Success { get; set; }
    }
}