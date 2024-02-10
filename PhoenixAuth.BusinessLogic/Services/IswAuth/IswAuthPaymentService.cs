using Microsoft.Extensions.Logging;
using System.Security.Authentication;
using PhoenixAuth.BusinessLogic.Extensions;
using PhoenixAuth.BusinessLogic.Models;
using PhoenixAuth.BusinessLogic.Services.Crypto;

namespace PhoenixAuth.BusinessLogic.Services.IswAuth
{
    public class IswAuthPaymentService: IIswAuthPaymentService
    {

        private readonly ILogger<IswAuthPaymentService> _logger;
        private readonly IIswAuthSignatureService _authSignature;
        private readonly ICryptoService _cryptoService;

        public IswAuthPaymentService(
            ICryptoService cryptoService, IIswAuthSignatureService authSignature,
            ILogger<IswAuthPaymentService> logger)
        {
            _cryptoService = cryptoService;
            _logger = logger;
            _authSignature = authSignature;
        }


        public (string decryptedCustReference, string decryptedWalletAccountId, string decryptedTerminalKey) 
            PaymentNotificationAuth(QtPaymentNotification model, PhoenixAuthHeaders authHeaders)
        {
            _logger.LogInformation(authHeaders.ToJson());

            //Validate ISW auth Params
            if (authHeaders == null)
                throw new AuthenticationException("Missing or empty signature to processor.");

            //Generate session key
            var sessionKey = _cryptoService.DoKeyExchange(authHeaders.ServerTransportKey, authHeaders.ProcessorTransportKey);

            // decrypt data with aes and session key
            var decryptedTerminalKey = _cryptoService.AesDecryption(model.PaymentDetail.TerminalId, sessionKey);
            var decryptedCustReference = _cryptoService.AesDecryption(model.PaymentDetail.CustReference, sessionKey);
            var decryptedWalletAccountId = _cryptoService.AesDecryption(model.PaymentDetail.WalletAccountId, sessionKey);

            //Generate processor signature
            var processorSignature =
                _authSignature.PaymentNotificationSignature(model.PaymentDetail, decryptedCustReference, decryptedTerminalKey);

            //_logger.LogInformation("{@processorSignature}", processorSignature);

            //Verify signature
            _cryptoService.VerifySignature(authHeaders.ProcessorTransportKey, authHeaders.ServerSignature, processorSignature);

            return (decryptedCustReference, decryptedWalletAccountId, decryptedTerminalKey);
        }
        
    }
}
