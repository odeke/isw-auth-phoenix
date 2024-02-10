using System;
using System.Net.Http;
using System.Threading.Tasks;
using static PhoenixAuth.BusinessLogic.Models.EndPoints;
using Newtonsoft.Json;
using PhoenixAuth.BusinessLogic.Extensions;
using PhoenixAuth.BusinessLogic.Models;
using PhoenixAuth.BusinessLogic.Services.Crypto;
using PhoenixAuth.BusinessLogic.Services.HttpClientWrapper;
using PhoenixAuth.BusinessLogic.Services.IswAuth;
using PhoenixAuth.BusinessLogic.Services.PartnerAuth;
using PhoenixAuth.BusinessLogic.Services.ReadConfiguration;

namespace PhoenixAuth.BusinessLogic.Services.KeyExchange
{
    public class KeyExchangeService: IKeyExchangeService
    {
        private readonly IIswAuthService _authService;
        private readonly ICryptoService _cryptoService;
        private readonly IReadConfigurationService _readConfiguration;
        private readonly IPhoenixHttpClient _httpClient;

        private readonly IPartnerAuthService _partnerAuth;

        public KeyExchangeService(IIswAuthService authService, ICryptoService cryptoService, 
            IReadConfigurationService readConfiguration, 
            IPhoenixHttpClient httpClient, IPartnerAuthService partnerAuth)
        {
            _authService = authService;
            _cryptoService = cryptoService;
            _readConfiguration = readConfiguration;
            _httpClient = httpClient;
            _partnerAuth = partnerAuth;
        }

        public async Task<ResponseResult> KeyExchangeAsync()
        {
            //Fetch private keys from the db
            var dbRecord = await _partnerAuth.GetPartnerAuthAsync(_readConfiguration.PhoenixClientId);

            var authParams = new PhoenixAuthParams
            {
                HttpMethod = HttpMethod.Post.Method,
                ResourceUrl = PostKeyExchangeUrl,
                PhoenixClientId = _readConfiguration.PhoenixClientId,
                PhoenixClientSecretKey = _readConfiguration.PhoenixClientSecretKey,
                PhoenixBaseUrl = _readConfiguration.PhoenixBaseUrl,
                PhoenixTerminalId = _readConfiguration.PhoenixTerminalId,
                AdditionalParameters = string.Empty
            };

            //Generate headers
            var headers = _authService.GenerateInterSwitchAuth(authParams, out var signatureCipher);
            //Sign signature cipher with rsa private
            headers[Signature] = _cryptoService.SignWithRsaPrivateKey(signatureCipher, dbRecord.RsaPrivateKey);

            // get new ECDH Keys 
            var ecdhKeys = _cryptoService.GetECDHKeys();

            // encrypt password 
            var requestReference = $"Broker_{new Random().Next(1000, 999900)}";
            var hashPassword = _cryptoService.ToSha512(_readConfiguration.PhoenixPassword) + requestReference + _readConfiguration.PhoenixSerialId;

            // Encrypt password with RSA private key 
            var aesHashPassword = _cryptoService.SignWithRsaPrivateKey(hashPassword, dbRecord.RsaPrivateKey);


            // Key exchange
            var request = new 
            {
                appVersion = "V1", //Must be the same as used on registration
                gprsCoordinate = "1.3119,32.4637", //Must be the same as used on registration
                terminalId = _readConfiguration.PhoenixTerminalId,
                password = aesHashPassword,
                requestReference = requestReference,
                serialId = _readConfiguration.PhoenixSerialId,
                clientSessionPublicKey = ecdhKeys.PublicKey,
            };

            var response = await _httpClient.PostJsonAsync(PostKeyExchangeUrl, request, headers);
            
            var result = await response.Content.ReadAsJsonAsync<ResponseResult>();
            if (result.ResponseCode != SwitchCodes.Success)
                return new ResponseResult
                {
                    ResponseCode = result.ResponseCode,
                    ResponseMessage = result.ResponseMessage
                };
            
            var resultObj = JsonConvert.DeserializeObject<PhoenixKeyExchange>(result.Response.ToJson());

            // Encrypt token with RSA private key
            var decryptedAuthToken = _cryptoService.DecryptWithRsaPrivateKey(resultObj.AuthToken, dbRecord.RsaPrivateKey);
            // Encrypt ServerSessionPublicKey with RSA private key
            var decrytedServerPublic = _cryptoService.DecryptWithRsaPrivateKey(resultObj.ServerSessionPublicKey, dbRecord.RsaPrivateKey);

            // Do key agreement decrytedServerPublic with ecdhKeys.Privatekey
            var sessionKey = _cryptoService.DoKeyExchange(ecdhKeys.PrivateKey, decrytedServerPublic);
            // Encrypt decryptedAuthToken with sessionKey
            var aesAuthToken = _cryptoService.DoAesEncryption(decryptedAuthToken, sessionKey);

            dbRecord.AuthToken = aesAuthToken;
            dbRecord.ExpireTime = resultObj.ExpireTime?? DateTime.UtcNow.AddMinutes(10);
            dbRecord.ServerSessionPublicKey = sessionKey;
            dbRecord.EcdhPrivateKey = ecdhKeys.PrivateKey;
            dbRecord.EcdhPublicKey = ecdhKeys.PublicKey;
            
            //Update db object for future use
            return await _partnerAuth.UpdatePartnerAuthAsync(dbRecord);
            
        }
    }
}
