namespace PhoenixAuth.BusinessLogic.Models
{
    public class PhoenixAuthHeaders
    {
        public string ServerSignature { get; set; }
        public string ServerTransportKey { get; set; }
        public string ProcessorTransportKey { get; set; }
    }

    public class PhoenixAuthParams
    {
        public string PhoenixBaseUrl { get; set; }
        public string PhoenixClientId { get; set; }
        public string PhoenixClientSecretKey { get; set; }
        public string PhoenixTerminalId { get; set; }
        public string HttpMethod { get; set; }
        public string ResourceUrl { get; set; }
        public string AdditionalParameters { get; set; }
    }
}
