using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using PhoenixAuth.BusinessLogic.Services.IswAuth;
using static PhoenixAuth.BusinessLogic.Extensions.ObjectExtensions;
using static PhoenixAuth.BusinessLogic.Models.EndPoints;

namespace PhoenixAuth.BusinessLogic.Services.HttpClientWrapper
{
    public class PhoenixHttpClient : IPhoenixHttpClient
    {
        private readonly IIswAuthService _interSwitchAuth;
        private HttpClient HttpClient { get; } 

        public PhoenixHttpClient(IHttpClientFactory clientFactory, IIswAuthService interSwitchAuth)
        {
            _interSwitchAuth = interSwitchAuth;
            HttpClient = clientFactory.CreateClient(PhoenixClient);
        }

        public async Task<HttpResponseMessage> GetAsync(string uri, Dictionary<string, string> headers)
        {
            var client = _interSwitchAuth.AddCustomHeaders(HttpClient, headers);
            return await client.GetAsync(uri);
        }
        
        //public async Task<HttpResponseMessage> PostJsonAsync<T>(string uri, T model, string additionalParameters = "")
        //{
        //    var client = _interSwitchAuth.AddCustomHeaders(HttpClient, HttpMethod.Post.Method, uri, additionalParameters);
        //    return await client.PostAsync(uri, model.ToHttpJsonContent());
        //}

        public async Task<HttpResponseMessage> PostJsonAsync<T>(string uri, T model, Dictionary<string, string> headers)
        {
            var client = _interSwitchAuth.AddCustomHeaders(HttpClient, headers);
            return await client.PostAsync(uri, model.ToHttpJsonContent());
        }
    }
}
