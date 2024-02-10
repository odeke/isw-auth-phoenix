namespace PhoenixAuth.BusinessLogic.Services.ReadConfiguration
{
    public interface IReadConfigurationService
    {

        public string DefaultConnection { get; }

        //Phoenix
        public string PhoenixBaseUrl { get; }
        public string PhoenixSerialId { get; }
        public string PhoenixClientId { get; }
        public string PhoenixPassword { get; }
        public string PhoenixTerminalId { get; }
        public string PhoenixTransportKey { get; }
        public string PhoenixClientSecretKey { get; }
    }
}
