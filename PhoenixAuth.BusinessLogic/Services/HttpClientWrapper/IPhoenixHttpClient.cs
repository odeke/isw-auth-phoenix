using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhoenixAuth.BusinessLogic.Services.HttpClientWrapper
{
    public interface IPhoenixHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string uri, Dictionary<string, string> headers);
        Task<HttpResponseMessage> PostJsonAsync<T>(string uri, T model, Dictionary<string, string> headers);
    }
}
