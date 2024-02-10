namespace PhoenixAuth.BusinessLogic.Models
{
    public static class EndPoints
    {
        public const string Signature = "Signature";
        public const string AuthToken = "AuthToken";

        public const string QtClient = "atbase";
        public const string PhoenixClient = "Phoenix";
        public const string PayPointClient = "PaypointClientApi";
        public const string PayPointSvaClient = "PaypointSVAClientApi";
        
        //Phoenix api
        public static string PostKeyExchangeUrl = "client/doKeyExchange";
        public static string PostClientRegistrationUrl = "client/clientRegistration";
        public static string PostCompleteClientRegistrationUrl = "client/completeClientRegistration";
        public static string QtCashOutUrl = "sente/xpayment"; //Auto_debit. Does not require validation
        
    }
}
