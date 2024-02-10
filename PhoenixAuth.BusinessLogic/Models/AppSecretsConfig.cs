namespace PhoenixAuth.BusinessLogic.Models
{
    public class AppSecretsConfig
    {

        //Db Con
        public string DefaultConnection { get; set; }

        
        //Phoenix
        public string PhoenixBaseUrl { get; set; }
        public string PhoenixSerialId { get; set; }
        public string PhoenixClientId { get; set; }
        public string PhoenixTerminalId { get; set; }
        public string PhoenixPassword { get; set; }
        public string PhoenixClientSecretKey { get; set; }
        public string PhoenixTransportKey { get; set; }


    }
}
