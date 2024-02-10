using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using static PhoenixAuth.BusinessLogic.Models.EndPoints;
using PhoenixAuth.BusinessLogic.Extensions;
using PhoenixAuth.BusinessLogic.Models;
using PhoenixAuth.BusinessLogic.Services.Crypto;
using PhoenixAuth.BusinessLogic.Services.HttpClientWrapper;
using PhoenixAuth.BusinessLogic.Services.IswAuth;
using PhoenixAuth.BusinessLogic.Services.PartnerAuth;
using PhoenixAuth.BusinessLogic.Services.ReadConfiguration;

namespace PhoenixAuth.BusinessLogic.Services.ClientRegistration
{
    public class ClientRegistrationService : IClientRegistrationService
    {
        private readonly ILogger<ClientRegistrationService> _logger;
        private readonly IIswAuthService _authService;
        private readonly ICryptoService _cryptoService;
        private readonly IReadConfigurationService _readConfiguration;
        private readonly IPhoenixHttpClient _httpClient;
        private readonly IPartnerAuthService _partnerAuth;

        public ClientRegistrationService(IPhoenixHttpClient httpClient, IReadConfigurationService readConfiguration,
            ICryptoService cryptoService, IIswAuthService authService, IPartnerAuthService partnerAuth,
            ILogger<ClientRegistrationService> logger)
        {
            _httpClient = httpClient;
            _readConfiguration = readConfiguration;
            _cryptoService = cryptoService;
            _authService = authService;
            _partnerAuth = partnerAuth;
            _logger = logger;
        }

        public async Task<ResponseResult> ClientRegistrationAsync()
        {
            // get new ECDH Keys 
            var ecdhKeys = _cryptoService.GetECDHKeys();

            var authParams = new PhoenixAuthParams
            {
                HttpMethod = HttpMethod.Post.Method,
                ResourceUrl = PostClientRegistrationUrl,
                PhoenixClientId = _readConfiguration.PhoenixClientId,
                PhoenixClientSecretKey = _readConfiguration.PhoenixClientSecretKey,
                PhoenixBaseUrl = _readConfiguration.PhoenixBaseUrl,
                PhoenixTerminalId = _readConfiguration.PhoenixTerminalId,
                AdditionalParameters = string.Empty
            };

            var headers = _authService.GenerateInterSwitchAuth(authParams, out var signatureCipher);
            //Get new RSA keys
            var rsaKeys = _cryptoService.GetRSAKeys();
            headers[Signature] = _cryptoService.SignWithRsaPrivateKey(signatureCipher, rsaKeys.PrivateKey);

            // client registration 
            var request = new Models.ClientRegistration
            {
                GprsCoordinate = "1.3119,32.4637",
                Name = "IIS Integration Broker",
                TerminalId = _readConfiguration.PhoenixTerminalId,
                AppVersion = "V1",
                PhoneNumber = "0774528787",
                Nin = "CM84082101GGJG",
                RequestReference = $"Broker_{new Random().Next(1000, 999900)}",
                SerialId = _readConfiguration.PhoenixSerialId,
                OwnerPhoneNumber = "0774528787",
                PublicKey = rsaKeys.PublicKey,// rsa public key 
                ClientSessionPublicKey = ecdhKeys.PublicKey // ecdh public key 
            };

            var response = await _httpClient.PostJsonAsync(PostClientRegistrationUrl, request, headers);

            var result = await response.Content.ReadAsJsonAsync<PhoenixBaseResponse>();

            if (result.ResponseCode != SwitchCodes.Success)
                return new ResponseResult { ResponseCode = result.ResponseCode, ResponseMessage = result.responseMessage };

            var securityManagerObj = new BusinessEntities.Models.Model.PartnerAuth
            {
                Environment = "TEST",
                RsaPrivateKey = rsaKeys.PrivateKey,
                RsaPublicKey = rsaKeys.PublicKey,
                Service = "CLIENT_REG",
                EcdhPrivateKey = ecdhKeys.PrivateKey,
                EcdhPublicKey = ecdhKeys.PublicKey,
                ClientId = _readConfiguration.PhoenixClientId,
                AuthToken = result?.response?.authToken,
                ServerSessionPublicKey = result?.response?.serverSessionPublicKey,
                TransactionReference = result?.response?.transactionReference
            };

            return await _partnerAuth.InsertPartnerAuthAsync(securityManagerObj);

        }
        
    }
}
