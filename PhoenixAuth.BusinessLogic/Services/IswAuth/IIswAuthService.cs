using System.Collections.Generic;
using System.Net.Http;
using PhoenixAuth.BusinessLogic.Models;

namespace PhoenixAuth.BusinessLogic.Services.IswAuth
{
    public interface IIswAuthService
    {
        HttpClient AddCustomHeaders(HttpClient client, Dictionary<string, string> headers);

        //HttpClient AddCustomHeaders(HttpClient client, string httpMethod, string resourceUrl,
        //    string additionalParameters = "");

        Dictionary<string, string> GenerateInterSwitchAuth(PhoenixAuthParams iswAuthParams, out string signatureCipher);

    }
}
