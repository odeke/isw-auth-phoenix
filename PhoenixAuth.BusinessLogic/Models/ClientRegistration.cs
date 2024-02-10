using Newtonsoft.Json;
using System;

namespace PhoenixAuth.BusinessLogic.Models
{
    public class ClientRegistration
    {
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }

        [JsonProperty("appVersion")]
        public string AppVersion { get; set; }

        [JsonProperty("serialId")]
        public string SerialId { get; set; }

        [JsonProperty("requestReference")]
        public string RequestReference { get; set; }

        [JsonProperty("gprsCoordinate")]
        public string GprsCoordinate { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("nin")]
        public string Nin { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("ownerPhoneNumber")]
        public string OwnerPhoneNumber { get; set; }

        [JsonProperty("publicKey")]
        public string PublicKey { get; set; }

        [JsonProperty("clientSessionPublicKey")]
        public string ClientSessionPublicKey { get; set; }
    }

    public class ClientRegistrationResponse
    {
        [JsonProperty("transactionReference")]
        public string TransactionReference { get; set; }

        [JsonProperty("authToken")]
        public string AuthToken { get; set; }

        [JsonProperty("serverSessionPublicKey")]
        public string ServerSessionPublicKey { get; set; }
    }

    public class Response
    {
        public string transactionReference { get; set; }
        public string authToken { get; set; }
        public string serverSessionPublicKey { get; set; }
    }

    public class PhoenixBaseResponse
    {
        public SwitchCodes ResponseCode { get; set; } = SwitchCodes.Success;
        //public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public object requestReference { get; set; }
        public Response response { get; set; }
    }

    public class PhoenixCompleteRegistration
    {
        [JsonProperty("terminalId")] public string TerminalId { get; set; }
        [JsonProperty("appVersion")] public string AppVersion { get; set; }
        [JsonProperty("serialId")] public string SerialId { get; set; }
        [JsonProperty("requestReference")] public string RequestReference { get; set; }
        [JsonProperty("gprsCoordinate")] public string GprsCoordinate { get; set; }
        [JsonProperty("otp")] public string Otp { get; set; }
        [JsonProperty("password")] public string Password { get; set; }
        [JsonProperty("transactionReference")] public string TransactionReference { get; set; }
    }


    public class CompleteRegistrationResponse
    {
        [JsonProperty("firstname")] public string Firstname { get; set; }
        [JsonProperty("lastname")] public string Lastname { get; set; }
        [JsonProperty("username")] public string Username { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("contact")] public string Contact { get; set; }
        [JsonProperty("authToken")] public string AuthToken { get; set; }
        [JsonProperty("expireTime")] public DateTime? ExpireTime { get; set; }
        [JsonProperty("merchantId")] public string MerchantId { get; set; }
        [JsonProperty("userId")] public string UserId { get; set; }
        [JsonProperty("terminalId")] public string TerminalId { get; set; }
        [JsonProperty("active")] public bool Active { get; set; }
        [JsonProperty("location")] public string Location { get; set; }
        [JsonProperty("operatorName")] public string OperatorName { get; set; }
        [JsonProperty("clientSecret")] public string ClientSecret { get; set; }
        [JsonProperty("serverSessionPublicKey")] public string ServerSessionPublicKey { get; set; }
        [JsonProperty("requiresOtp")] public bool RequiresOtp { get; set; }
        [JsonProperty("tpk")] public string Tpk { get; set; }
        [JsonProperty("tpkIV")] public string TpkIV { get; set; }
        [JsonProperty("currencySymbol")] public string CurrencySymbol { get; set; }
        [JsonProperty("currencyCode")] public string CurrencyCode { get; set; }
        [JsonProperty("countryCode")] public string CountryCode { get; set; }
        [JsonProperty("clientTerminalId")] public string ClientTerminalId { get; set; }
    }
}
