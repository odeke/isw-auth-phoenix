using System;

namespace PhoenixAuth.BusinessLogic.BusinessEntities.Models.Model
{
    public class PartnerAuth : ModelBase
    {
        public string ClientId { get; set; }
        public string AuthToken { get; set; }
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// SessionKey
        /// </summary>
        public string ServerSessionPublicKey { get; set; }
        public string TransactionReference { get; set; }
        public string EcdhPublicKey { get; set; }
        public string EcdhPrivateKey { get; set; }
        public string Service { get; set; }
        public string EncryptionType { get; set; }
        public string RsaPublicKey { get; set; }
        public string RsaPrivateKey { get; set; }
        public string Environment { get; set; }

    }
}
