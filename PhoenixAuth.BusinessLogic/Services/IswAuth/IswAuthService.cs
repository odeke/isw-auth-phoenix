using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using PhoenixAuth.BusinessLogic.Extensions;
using PhoenixAuth.BusinessLogic.Models;

namespace PhoenixAuth.BusinessLogic.Services.IswAuth
{
    public class IswAuthService : IIswAuthService
    {
        private readonly ILogger<IswAuthService> _logger;

        public IswAuthService(ILogger<IswAuthService> logger)
        {
            _logger = logger;
        }

        private const string Timestamp = "Timestamp";
        private const string Nonce = "Nonce";
        private const string SignatureMethod = "SHA-256";
        private const string SignatureMethodKey = "SignatureMethod";
        private const string Signature = "Signature";
        private const string Authorization = "Authorization";
        private const string TerminalIdKey = "TerminalId";
        private const string AuthorizationRealm = "InterswitchAuth";

        public HttpClient AddCustomHeaders(HttpClient client, Dictionary<string, string> headers)
        {
            foreach (var keyValuePair in headers)
                client.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);

            return client;
        }
        //TO-DO: Add validation to this method
        public Dictionary<string, string> GenerateInterSwitchAuth(PhoenixAuthParams iswAuthParams, out string signatureCipher)
        {
            var interSwitchAuth = new Dictionary<string, string>();

            var resourceUrl = $"{iswAuthParams.PhoenixBaseUrl}{iswAuthParams.ResourceUrl}";

            // Timestamp must be in seconds.
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();//This also works without computations

            var nonce = Guid.NewGuid().ToString().Replace("-", "");

            var clientIdBase64 = iswAuthParams.PhoenixClientId.ToBase64Encode();
            var authorization = $"{AuthorizationRealm} {clientIdBase64}";

            var encodedResourceUrl = System.Net.WebUtility.UrlEncode(resourceUrl);
            signatureCipher = $"{iswAuthParams.HttpMethod}&{encodedResourceUrl}&{timestamp}&{nonce}&{iswAuthParams.PhoenixClientId}&{iswAuthParams.PhoenixClientSecretKey}";

            if (!string.IsNullOrWhiteSpace(iswAuthParams.AdditionalParameters))
                signatureCipher = $"{signatureCipher}&{iswAuthParams.AdditionalParameters}";
            _logger.LogInformation(signatureCipher);
            // encode signature as base 64 
            //string signature = signatureCipher.SHA256();

            interSwitchAuth.Add(Authorization, authorization);
            interSwitchAuth.Add(Timestamp, timestamp);
            interSwitchAuth.Add(Nonce, nonce);
            interSwitchAuth.Add(SignatureMethodKey, SignatureMethod);
            interSwitchAuth.Add(Signature, signatureCipher);
            interSwitchAuth.Add(TerminalIdKey, iswAuthParams.PhoenixTerminalId);

            return interSwitchAuth;
        }
        
    }
}
