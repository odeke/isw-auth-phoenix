using Microsoft.Extensions.Logging;
using System.Security.Authentication;
using PhoenixAuth.BusinessLogic.Extensions;
using PhoenixAuth.BusinessLogic.Models;
using PhoenixAuth.BusinessLogic.Services.Crypto;

namespace PhoenixAuth.BusinessLogic.Services.IswAuth
{
    public class IswAuthValidationService: IIswAuthValidationService
    {
        private readonly ILogger<IswAuthValidationService> _logger;
        private readonly IIswAuthSignatureService _authSignature;
        private readonly ICryptoService _cryptoService;

        public IswAuthValidationService(
            IIswAuthSignatureService authSignature, ICryptoService cryptoService,
            ILogger<IswAuthValidationService> logger)
        {
            _authSignature = authSignature;
            _cryptoService = cryptoService;
            _logger = logger;
        }

        public (string decryptedCustReference, string decryptedTerminalKey) 
            ValidationAuth(QtAccountValidation model, PhoenixAuthHeaders authHeaders)
        {

            _logger.LogInformation(authHeaders.ToJson());

            //Validate ISW auth Params
            if (authHeaders == null)
                throw new AuthenticationException("Missing or empty signature to processor.");

            //Generate session key
            var sessionKey = _cryptoService.DoKeyExchange(authHeaders.ServerTransportKey, authHeaders.ProcessorTransportKey);

            // decrypt data with aes and session key
            var decryptedTerminalKey = _cryptoService.AesDecryption(model.CustomerDetail.TerminalId, sessionKey);
            var decryptedCustReference = _cryptoService.AesDecryption(model.CustomerDetail.CustReference, sessionKey);

            //Generate processor signature
            var processorSignature =
                _authSignature.ValidationSignature(model.CustomerDetail, decryptedCustReference, decryptedTerminalKey);

            //_logger.LogInformation("{@processorSignature}", processorSignature);

            //Verify signature
            _cryptoService.VerifySignature(authHeaders.ProcessorTransportKey, authHeaders.ServerSignature, processorSignature);

            return (decryptedCustReference, decryptedTerminalKey);
        }
    }
}
