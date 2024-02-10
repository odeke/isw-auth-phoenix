using Newtonsoft.Json;
using System;

namespace PhoenixAuth.BusinessLogic.Models
{
    public class EncryptionKeys
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }

    public class PhoenixKeyExchange
    {
        [JsonProperty("authToken")] public string AuthToken { get; set; }

        [JsonProperty("terminalKey")] public object TerminalKey { get; set; }

        [JsonProperty("expireTime")] public DateTime? ExpireTime { get; set; } = DateTime.UtcNow.AddMinutes(10);

        [JsonProperty("requiresOtp")] public bool RequiresOtp { get; set; }

        [JsonProperty("serverSessionPublicKey")] public string ServerSessionPublicKey { get; set; }
    }
}
