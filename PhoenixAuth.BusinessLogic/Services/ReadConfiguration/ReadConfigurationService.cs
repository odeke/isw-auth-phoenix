using Microsoft.Extensions.Options;
using PhoenixAuth.BusinessLogic.Models;

namespace PhoenixAuth.BusinessLogic.Services.ReadConfiguration
{
    public class ReadConfigurationService : IReadConfigurationService
    {
        //https://joonasw.net/view/aspnet-core-2-configuration-changes
        //http://www.c-sharpcorner.com/article/setting-and-reading-values-from-app-settings-json-in-net-core/
        //https://stackoverflow.com/questions/31453495/how-to-read-appsettings-values-from-config-json-in-asp-net-core
        //https://stackoverflow.com/questions/41695889/access-appsettings-json-file-setting-from-the-class-library-project
        //https://hassantariqblog.wordpress.com/2017/02/20/asp-net-core-step-by-step-guide-to-access-appsettings-json-in-web-project-and-class-library/

        private readonly IOptions<AppSecretsConfig> _options;
        public ReadConfigurationService(IOptions<AppSecretsConfig> options)
        {
            _options = options;
        }

        //Db Con
        public string DefaultConnection => _options.Value.DefaultConnection;


        //Phoenix
        public string PhoenixBaseUrl => _options.Value.PhoenixBaseUrl;
        public string PhoenixSerialId => _options.Value.PhoenixSerialId;
        public string PhoenixClientId => _options.Value.PhoenixClientId;
        public string PhoenixPassword => _options.Value.PhoenixPassword;
        public string PhoenixTerminalId => _options.Value.PhoenixTerminalId;
        public string PhoenixTransportKey => _options.Value.PhoenixTransportKey;
        public string PhoenixClientSecretKey => _options.Value.PhoenixClientSecretKey;

    }
}
