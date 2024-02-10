using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PhoenixAuth.BusinessLogic.Models;
using PhoenixAuth.BusinessLogic.Services.Crypto;
using PhoenixAuth.BusinessLogic.Services.Helper;
using PhoenixAuth.BusinessLogic.Services.HttpClientWrapper;
using PhoenixAuth.BusinessLogic.Services.IswAuth;
using PhoenixAuth.BusinessLogic.Services.PartnerAuth;
using PhoenixAuth.BusinessLogic.Services.ReadConfiguration;
using static PhoenixAuth.BusinessLogic.Models.EndPoints;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using PhoenixAuth.BusinessLogic.Extensions;

namespace PhoenixAuth.BusinessLogic.Services.CompleteRegistration
{
    public class CompleteRegistration: ICompleteRegistration
    {

        private readonly IWritableOptions<AppSecretsConfig> _writableOptions;
        private readonly ILogger<CompleteRegistration> _logger;
        private readonly IIswAuthService _authService;
        private readonly ICryptoService _cryptoService;
        private readonly IReadConfigurationService _readConfiguration;
        private readonly IPhoenixHttpClient _httpClient;

        private readonly IPartnerAuthService _partnerAuth;

        public CompleteRegistration(ILogger<CompleteRegistration> logger, IIswAuthService authService, 
            ICryptoService cryptoService, IReadConfigurationService readConfiguration, IPhoenixHttpClient httpClient, 
            IPartnerAuthService partnerAuth, IWritableOptions<AppSecretsConfig> writableOptions)
        {
            _logger = logger;
            _authService = authService;
            _cryptoService = cryptoService;
            _readConfiguration = readConfiguration;
            _httpClient = httpClient;
            _partnerAuth = partnerAuth;
            _writableOptions = writableOptions;
        }

        public async Task<ResponseResult> CompleteClientRegistrationAsync()
        {
            //_coreValidation.Validation(new StringValidator(), otp);

            //Fetch private keys from the db
            var dbRecord = await _partnerAuth.GetPartnerAuthAsync(_readConfiguration.PhoenixClientId);

            // decrypt AuthToken with RSAPrivateKey
            var decryptedAuth = _cryptoService.DecryptWithRsaPrivateKey(dbRecord.AuthToken, dbRecord.RsaPrivateKey);
            // decrypt ServerSessionPublicKey with RSAPrivateKey
            var decrytedServerPublic = _cryptoService.DecryptWithRsaPrivateKey(dbRecord.ServerSessionPublicKey, dbRecord.RsaPrivateKey);

            // key exchange
            var sessionKey = _cryptoService.DoKeyExchange(dbRecord.EcdhPrivateKey, decrytedServerPublic);
            // encrypt decryptedAuth with sessionKey using AES
            var aesAuthToken = _cryptoService.DoAesEncryption(decryptedAuth, sessionKey);
            // encrypt otp with sessionKey using AES
            //var aesOtp = _cryptoService.DoAesEncryption(otp, sessionKey);
            // hash IswSecurityPassword with sha512
            var hashPassword = _cryptoService.ToSha512(_readConfiguration.PhoenixPassword);
            // encrypt hash Password with sessionKey using AES
            var aesHashPassword = _cryptoService.DoAesEncryption(hashPassword, sessionKey);

            var authParams = new PhoenixAuthParams
            {
                HttpMethod = HttpMethod.Post.Method,
                ResourceUrl = PostCompleteClientRegistrationUrl,
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
            headers.Add(AuthToken, aesAuthToken);

            // client registration 
            var request = new PhoenixCompleteRegistration
            {
                GprsCoordinate = "1.3119,32.4637",//Your current coordinates
                TerminalId = _readConfiguration.PhoenixTerminalId,
                AppVersion = "V1",
                RequestReference = $"Broker_{new Random().Next(1000, 999900)}",
                SerialId = _readConfiguration.PhoenixSerialId,
                //Otp = aesOtp,
                Password = aesHashPassword,
                TransactionReference = dbRecord.TransactionReference,
            };

            //Post request to the server
            var response = await _httpClient.PostJsonAsync(PostCompleteClientRegistrationUrl, request, headers);
            //Read server response
            var result = await response.Content.ReadAsJsonAsync<ResponseResult>();

            if (result.ResponseCode != SwitchCodes.Success)
                return new ResponseResult { ResponseCode = result.ResponseCode, ResponseMessage = result.ResponseMessage };

            var resultObj = JsonConvert.DeserializeObject<CompleteRegistrationResponse>(result.Response.ToJson());

            //Get new client secrete
            var secretKey = _cryptoService.DecryptWithRsaPrivateKey(resultObj.ClientSecret, dbRecord.RsaPrivateKey);
            _logger.LogInformation($"New Client Secrete: {secretKey}");

            //Update the db with the new authentication token details
            dbRecord.AuthToken = aesAuthToken;
            dbRecord.ExpireTime = resultObj.ExpireTime ?? DateTime.Now.AddHours(2);

            //Update the client secrete
            _writableOptions.Update(opt =>
            {
                opt.PhoenixClientSecretKey = secretKey;
            });

            return await _partnerAuth.UpdatePartnerAuthAsync(dbRecord);

        }
    }
}
