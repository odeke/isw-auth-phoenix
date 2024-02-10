using System;
using System.Threading.Tasks;
using static PhoenixAuth.BusinessLogic.Models.EndPoints;
using System.Net.Http;
using PhoenixAuth.BusinessLogic.Extensions;
using PhoenixAuth.BusinessLogic.Models;
using PhoenixAuth.BusinessLogic.Services.Crypto;
using PhoenixAuth.BusinessLogic.Services.HttpClientWrapper;
using PhoenixAuth.BusinessLogic.Services.IswAuth;
using PhoenixAuth.BusinessLogic.Services.KeyExchange;
using PhoenixAuth.BusinessLogic.Services.PartnerAuth;
using PhoenixAuth.BusinessLogic.Services.ReadConfiguration;

namespace PhoenixAuth.BusinessLogic.Services.CashOut
{
    public class CashOutService : ICashOutService
    {
        private readonly IKeyExchangeService _exchangeService;

        private readonly IPartnerAuthService _partnerAuth;
        private readonly IIswAuthService _authService;
        private readonly ICryptoService _cryptoService;
        private readonly IReadConfigurationService _readConfiguration;
        private readonly IPhoenixHttpClient _httpClient;

        public CashOutService(IPhoenixHttpClient httpClient, IReadConfigurationService readConfiguration,
            IIswAuthService authService, IPartnerAuthService partnerAuth,
            ICryptoService cryptoService, IKeyExchangeService exchangeService)
        {
            _httpClient = httpClient;
            _readConfiguration = readConfiguration;
            _authService = authService;
            _partnerAuth = partnerAuth;
            _cryptoService = cryptoService;
            _exchangeService = exchangeService;
        }

        public async Task<ResponseResult> CashWithdrawAsync(PhoenixCashOutModel model)
        {
            model.TerminalId = _readConfiguration.PhoenixTerminalId;
            
            //Generate headers
            var additionalParams = $"{model.Amount}&{model.TerminalId}&{model.RequestReference}&{model.CustomerId}&{model.PaymentCode}";//Also works

            var authParams = new PhoenixAuthParams
            {
                HttpMethod = HttpMethod.Post.Method,
                ResourceUrl = QtCashOutUrl,
                PhoenixClientId = _readConfiguration.PhoenixClientId,
                PhoenixClientSecretKey = _readConfiguration.PhoenixClientSecretKey,
                PhoenixBaseUrl = _readConfiguration.PhoenixBaseUrl,
                PhoenixTerminalId = _readConfiguration.PhoenixTerminalId,
                AdditionalParameters = additionalParams
            };

            //Generate headers
            var headers = _authService.GenerateInterSwitchAuth(authParams, out var signatureCipher);
            
            //Fetch private keys from the db
            var dbRecord = await _partnerAuth.GetPartnerAuthAsync(_readConfiguration.PhoenixClientId);

            //Check if token is still valid otherwise get a new one
            if (dbRecord.ExpireTime < DateTime.Now)
            {
                var keyExchangeResult = await _exchangeService.KeyExchangeAsync();
                if (keyExchangeResult.ResponseCode != SwitchCodes.Success) return keyExchangeResult;

                //Fetch new keys from the db
                dbRecord = await _partnerAuth.GetPartnerAuthAsync(_readConfiguration.PhoenixClientId);
            }

            //Sign signature cipher with rsa private
            headers[Signature] = _cryptoService.SignWithRsaPrivateKey(signatureCipher, dbRecord.RsaPrivateKey);
            headers.Add(AuthToken, dbRecord.AuthToken);

            var response = await _httpClient.PostJsonAsync(QtCashOutUrl, model, headers);

            return await response.Content.ReadAsJsonAsync<ResponseResult>();
            

        }
    }

}
