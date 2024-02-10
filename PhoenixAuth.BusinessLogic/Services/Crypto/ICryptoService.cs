using PhoenixAuth.BusinessLogic.Models;

namespace PhoenixAuth.BusinessLogic.Services.Crypto
{
    public interface ICryptoService
    {
        /// <summary>
        /// Generate new ECDH Keys
        /// </summary>
        /// <returns></returns>
        EncryptionKeys GetECDHKeys();

        /// <summary>
        /// Decrypt with RSA Private key
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="rsaPrivateKey"></param>
        /// <returns></returns>
        string DecryptWithRsaPrivateKey(string cipherText, string rsaPrivateKey);

        /// <summary>
        /// Do Data Key exchange or Key Agreement
        /// </summary>
        /// <param name="eCDHPrivateKey"></param>
        /// <param name="serverSessionPublicKey"></param>
        /// <returns></returns>
        string DoKeyExchange(string eCDHPrivateKey, string serverSessionPublicKey);

        /// <summary>
        /// Encrypt data with AED algo using sessionKey
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        string DoAesEncryption(string data, string sessionKey);

        ///// <summary>
        ///// Decrypt with AED algo using sessionKey
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="sessionKey"></param>
        ///// <returns></returns>
        //string DoAesDecryption(string data, string sessionKey);

        /// <summary>
        /// Do sha512
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string ToSha512(string source);

        /// <summary>
        /// Generate new RSA Keys
        /// </summary>
        /// <returns></returns>
        EncryptionKeys GetRSAKeys();

        /// <summary>
        /// Do Rsa signature with RSAKeyPrameter. Sign With Rsa PrivateKey
        /// </summary>
        /// <param name="sourceData"></param>
        /// <param name="rsaPrivateKey"></param>
        /// <returns></returns>
        string SignWithRsaPrivateKey(string sourceData, string rsaPrivateKey);

        /// <summary>
        /// Decrypt data with AES algo using sessionKey
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        string AesDecryption(string data, string sessionKey);

        /// <summary>
        /// Verify server signature at the processor before request execution. 
        /// </summary>
        /// <param name="ecdhPublicKey">Processor Transport key (ECDH Public Key)</param>
        /// <param name="serverSignature">Server generated signature</param>
        /// <param name="plainText">Processor generated Signature</param>
        /// <returns>Returns true or false depending on the verification status</returns>
        bool VerifySignatureCore(string ecdhPublicKey, string serverSignature, string plainText);

        /// <summary>
        /// Verify server signature at the processor before request execution.
        /// Internally calls VerifySignatureCore(...) methods. Throws AuthenticationException if validation is false.
        /// </summary>
        /// <param name="ecdhPublicKey">Processor Transport key (ECDH Public Key)</param>
        /// <param name="serverSignature">Server generated signature</param>
        /// <param name="plainText">Processor generated Signature</param>
        void VerifySignature(string ecdhPublicKey, string serverSignature, string plainText);
    }
}
