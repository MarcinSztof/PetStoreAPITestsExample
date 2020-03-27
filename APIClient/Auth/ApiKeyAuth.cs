using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace APIClient.Auth
{
    public class ApiKeyAuth : IAuthModule
    {
        public string ApiKey { get; private set; }

        public ApiKeyAuth(string apiKey)
        {
            ApiKey = apiKey;
        }

        public HttpRequestMessage AddAuthHeader(HttpRequestMessage request)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("api_key", ApiKey);
            return request;
        }
    }
}
