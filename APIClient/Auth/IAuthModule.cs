using System.Net.Http;

namespace APIClient.Auth
{
    public interface IAuthModule
    {
        HttpRequestMessage AddAuthHeader(HttpRequestMessage request);
    }
}
