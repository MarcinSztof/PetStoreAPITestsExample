using System.Net.Http;

namespace APIClient.Auth
{
    public class NoAuthModule : IAuthModule
    {
        public HttpRequestMessage AddAuthHeader(HttpRequestMessage request) => request;
    }
}
