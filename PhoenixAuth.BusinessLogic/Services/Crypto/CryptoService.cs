using System;
using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using PhoenixAuth.BusinessLogic.Models;
using static PhoenixAuth.BusinessLogic.Models.UserMessages;
using static PhoenixAuth.BusinessLogic.Extensions.ObjectExtensions;

namespace PhoenixAuth.BusinessLogic.Services.Crypto
{
    public class CryptoService : ICryptoService
    {
        private readonly ILogger<CryptoService> _logger;

        public CryptoService(ILogger<CryptoService> logger)
        {
            _logger = logger;
        }

        public EncryptionKeys GetECDHKeys()
        {
            ECKeyPairGenerator gen = new("ECDH");
            SecureRandom secureRandom = new();
            X9ECParameters ecp = NistNamedCurves.GetByName("P-256");
            ECDomainParameters ecSpec = new(ecp.Curve, ecp.G, ecp.N, ecp.H, ecp.GetSeed());
            ECKeyGenerationParameters ecgp = new(ecSpec, secureRandom);
            gen.Init(ecgp);
            AsymmetricCipherKeyPair eckp = gen.GenerateKeyPair();

            ECPublicKeyParameters ecPub = (ECPublicKeyParameters)eckp.Public;
            ECPrivateKeyParameters ecPri = (ECPrivateKeyParameters)eckp.Private;

            var publicKeyBytes = ecPub.Q.GetEncoded();
            var privateKeyBytes = ecPri.D.ToByteArray();

            var privateKey = Convert.ToBase64String(privateKeyBytes);
            var publicKey = Convert.ToBase64String(publicKeyBytes);

            EncryptionKeys response = new()
            {
                PrivateKey = privateKey,
                PublicKey = publicKey,
            };

            return response;
        }

        public string DecryptWithRsaPrivateKey(string cipherText, string rsaPrivateKey)
        {
            try
            {
                var decryptionKey = (RsaKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(rsaPrivateKey));

                IAsymmetricBlockCipher eng = new OaepEncoding(new RsaEngine(), new Sha256Digest(), new Sha256Digest(), null);
                eng.Init(false, decryptionKey);
                var encdata = Convert.FromBase64String(cipherText);
                encdata = eng.ProcessBlock(encdata, 0, encdata.Length);
                var result = Encoding.UTF8.GetString(encdata);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }

        /// <summary>
        ///  Do Data Key exchange or Key Agreement
        /// </summary>
        /// <param name="eCDHPrivateKey"></param>
        /// <param name="serverSessionPublicKey"></param>
        /// <returns></returns>
        public string DoKeyExchange(string eCDHPrivateKey, string serverSessionPublicKey)
        {
            var curve = ECNamedCurveTable.GetByName("prime256v1");
            var ecParam = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            byte[] pubkicKeyBtye = Convert.FromBase64String(serverSessionPublicKey);
            var point = ecParam.Curve.DecodePoint(pubkicKeyBtye);
            var pubKey = new ECPublicKeyParameters(point, ecParam);
            var agreement = AgreementUtilities.GetBasicAgreement("ECDH");
            var privateByte = Convert.FromBase64String(eCDHPrivateKey);

            var privKey = new ECPrivateKeyParameters(new BigInteger(privateByte, 0, privateByte.Length), ecParam);

            agreement.Init(privKey);
            BigInteger secret = agreement.CalculateAgreement(pubKey);

            string sessionKey = Convert.ToBase64String(secret.ToByteArrayUnsigned());

            return sessionKey;
        }

        /// <summary>
        /// Encrypt data with AED algo using sessionKey
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public string DoAesEncryption(string data, string sessionKey)
        {
            using MemoryStream ms = new();
            using AesManaged cryptor = new();
            cryptor.Mode = CipherMode.CBC;
            cryptor.Padding = PaddingMode.PKCS7;
            cryptor.KeySize = 128;
            cryptor.BlockSize = 128;

            //We use the random generated iv created by AesManaged
            byte[] iv = cryptor.IV;
            var dataBtye = Encoding.ASCII.GetBytes(data);

            using (CryptoStream cs = new(ms, cryptor.CreateEncryptor(Convert.FromBase64String(sessionKey), iv), CryptoStreamMode.Write))
            {
                cs.Write(dataBtye, 0, dataBtye.Length);
            }
            byte[] encryptedContent = ms.ToArray();

            //Create new byte array that should contain both unencrypted iv and encrypted data
            byte[] result = new byte[iv.Length + encryptedContent.Length];

            //copy our 2 array into one
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encryptedContent, 0, result, iv.Length, encryptedContent.Length);

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Generate new RSA Keys
        /// </summary>
        /// <returns></returns>
        public EncryptionKeys GetRSAKeys()
        {
            RsaKeyPairGenerator rsaKeyPairGenerator = new();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));

            AsymmetricCipherKeyPair keys = rsaKeyPairGenerator.GenerateKeyPair();

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keys.Private);
            // Write out an RSA private key with it's asscociated information as described in PKCS8.
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();
            // Convert to Base64 ..
            string serializedPrivateString = Convert.ToBase64String(serializedPrivateBytes);

            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keys.Public);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            string serializedPublicString = Convert.ToBase64String(serializedPublicBytes);
            
            EncryptionKeys response = new()
            {
                PrivateKey = ConvertPemToBase64String(serializedPrivateString, true).Trim(),
                PublicKey = ConvertPemToBase64String(serializedPublicString, false).Trim(),
            };

            return response;
        }

        /// <summary>
        ///  Convert Pem to base 64
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isPrivate"></param>
        /// <returns></returns>
        private static string ConvertPemToBase64String(string data, bool isPrivate)
        {

            string privateHeaderr = "-----BEGIN RSA PRIVATE KEY-----";
            string privateFooter = "-----END RSA PRIVATE KEY-----";


            string publicHeaderr = "-----BEGIN PUBLIC KEY-----";
            string publicHFooter = "-----END PUBLIC KEY-----";

            if (isPrivate)
                data = data.Replace(privateHeaderr, "").Replace(privateFooter, "");

            data = data.Replace(publicHeaderr, "").Replace(publicHFooter, "");

            return data;
        }

        /// <summary>
        /// Do Rsa signature with RSAKeyPrameter. Sign With Rsa PrivateKey
        /// </summary>
        /// <param name="sourceData"></param>
        /// <param name="rsaPrivateKey"></param>
        /// <returns></returns>
        public string SignWithRsaPrivateKey(string sourceData, string rsaPrivateKey)
        {
            RsaKeyParameters privateKey = (RsaKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(rsaPrivateKey));

            var tmpSource = Encoding.ASCII.GetBytes(sourceData);

            ISigner sign = SignerUtilities.GetSigner(PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id);
            sign.Init(true, privateKey);
            sign.BlockUpdate(tmpSource, 0, tmpSource.Length);
            return Convert.ToBase64String(sign.GenerateSignature());
        }

        /// <summary>
        ///  Do sha512
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string ToSha512(string source)
        {
            using SHA512 sha512Hash = SHA512.Create();
            //From String to byte array
            var sourceBytes = Encoding.UTF8.GetBytes(source);
            var hashBytes = sha512Hash.ComputeHash(sourceBytes);
            var hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

            return hash;
        }

        public string AesDecryption(string data, string sessionKey)
        {

            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Key = Convert.FromBase64String(sessionKey);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                var rawData = Convert.FromBase64String(data);
                var IV = new byte[16];  // aes.IV.Length should be 16

                Array.Copy(rawData, IV, IV.Length);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(aes.Key, IV), CryptoStreamMode.Write))
                    {
                        using (var binaryWriter = new BinaryWriter(cs))
                        {
                            binaryWriter.Write(rawData, IV.Length, rawData.Length - IV.Length);
                        }
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        public bool VerifySignatureCore(string ecdhPublicKey, string serverSignature, string plainText)
        {
            try
            {
                var curve = ECNamedCurveTable.GetByName("prime256v1");
                var ecParam = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
                var publicKeyByte = Convert.FromBase64String(ecdhPublicKey);
                var point = ecParam.Curve.DecodePoint(publicKeyByte);
                var pubKey = new ECPublicKeyParameters(point, ecParam);


                var msgBytes = Encoding.UTF8.GetBytes(plainText);
                var sigBytes = Convert.FromBase64String(serverSignature);

                ISigner signer = SignerUtilities.GetSigner("SHA256withECDSA");
                signer.Init(false, pubKey);
                signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
                return signer.VerifySignature(sigBytes);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Signature verification failed with the error: " + exc.InnerException?.ToJson());
                return false;
            }
        }

        public void VerifySignature(string ecdhPublicKey, string serverSignature, string plainText)
        {
            var isValid = this.VerifySignatureCore(ecdhPublicKey, serverSignature, plainText);
            if (!isValid) throw new AuthenticationException(SignatureVerification);
        }

        //public bool VerifySignature(String signature, String plaintext, PublicKey publicKey)
        //{
        //    string SIGNATURE_ALGORITHM = "SHA256withECDSA";
        //    Signature ecdsaVerify = Signature.GetInstance(SIGNATURE_ALGORITHM, true, "BC");
        //    ecdsaVerify.initVerify(publicKey);
        //    ecdsaVerify..update(plaintext.getBytes(Encoding.UTF8));
        //    return ecdsaVerify.verify(Base64.decodeBase64(signature));

        //    return false;
        //}


        //public bool VerifySignature(string PublicKey, string Signature, string Message)
        //{
        //    try
        //    {
        //        AsymmetricKeyParameter pubKey = new AsymmetricKeyParameter(false);

        //        var publicKey = PublicKeyFactory.CreateKey(Convert.FromBase64String(PublicKey));
        //        ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
        //        byte[] orgBytes = UnicodeEncoding.ASCII.GetBytes(Message);

        //        signer.Init(false, publicKey);
        //        signer.BlockUpdate(orgBytes, 0, orgBytes.Length);

        //        //Base64 Decode
        //        byte[] encodeBytes = UnicodeEncoding.ASCII.GetBytes(Signature);
        //        byte[] decodeBytes;
        //        using (MemoryStream decStream = new MemoryStream())
        //        {
        //            Base64.Decode(encodeBytes, 0, encodeBytes.Length, decStream);
        //            decodeBytes = decStream.ToArray();
        //        }

        //        return signer.VerifySignature(decodeBytes);
        //    }
        //    catch (Exception exc)
        //    {
        //        _logger.LogError(exc, "Verification failed with the error: " + exc.InnerException);
        //        return false;
        //    }
        //}


    }
}
