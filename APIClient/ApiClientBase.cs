using APIClient.Auth;
using APIClient.Serializers;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace APIClient
{
    public abstract class ApiClientBase
    {
        protected static readonly HttpClient HttpClient = new HttpClient(); // It is thread-safe, and using single instance for the whole life of an application makes better use of sockets
        public readonly Protocol Protocol;
        public readonly string BaseApiPath;
        public string BaseUrl => Protocol.ToString().ToLower() + "://" + BaseApiPath;
        protected IAuthModule AuthModule;
        protected ISerializer Serializer;
        protected RequestBuilder RequestBuilder;
        public ResponseStatusCategory[] AcceptedResponseStatuses { get; set; }
        public HttpResponseMessage LastResponse { get; private set; }

        protected ApiClientBase(ClientConfig config)
        {
            Protocol = config.Protocol;
            BaseApiPath = config.ApiPath;
            AuthModule = config.AuthModule;
            Serializer = config.Serializer;
            RequestBuilder = config.RequestBuilder;
            AcceptedResponseStatuses = config.AcceptedResponseStatuses;
        }

        public HttpResponseMessage SendRequest(HttpRequestMessage request)
        {
            LastResponse = HttpClient.SendAsync(AuthModule.AddAuthHeader(request)).Result;
            if (!IsStatusAccepted(LastResponse.StatusCode))
            {
                Console.WriteLine($"ERROR: Status from response is {LastResponse.StatusCode.ToString("d")}, accepted statuses are: {AcceptedResponseStatuses.Select(x => x.ToString("d").Replace("0", "*"))}");
                throw new Exception("Invalid Status"); // TODO: to be changed for custom exception, alternatiely nunit assertion could be used here and would be even better but then nunit dependency would be required for client and maybe better not to do so
            }
            return LastResponse;
        }

        public HttpResponseMessage SendRequest(HttpMethod method, string path) => SendRequest(RequestBuilder.BuildRequest(method, new Uri(BaseUrl + path)));

        public HttpResponseMessage SendRequest(HttpMethod method, string path, string body)
        {
            using(var request = RequestBuilder.BuildRequest(method, new Uri(BaseUrl + path), body)) // Here it's just a string content in a request returned so it dooesn't really matter, but disposing it anyway so when in the future it would be refactored to return requests with content that needs to be disposed, the using block will be reminder of that
            {
                return SendRequest(request);
            }     
        }
            
        public TResponseBody SendRequest<TResponseBody>(HttpMethod method, string path) => Serializer.Deserialize<TResponseBody>(SendRequest(method, path).Content.ReadAsStringAsync().Result);

        public void SendRequest(HttpMethod method, string path, object requestBody) => SendRequest(method, path, Serializer.Serialize(requestBody));

        public TResponseBody SendRequest<TResponseBody>(HttpMethod method, string path, object requestBody) => 
            Serializer.Deserialize<TResponseBody>(SendRequest(method, path, Serializer.Serialize(requestBody)).Content.ReadAsStringAsync().Result);

        private bool IsStatusAccepted(HttpStatusCode receivedStatus) => AcceptedResponseStatuses.Any(x => (int)x / 100 == (int)receivedStatus / 100); // So if Status400 is one of the accepted, then any 4** is accepted

        public Uri GetFullUrl(string endpoint) => new Uri(BaseUrl + "/" + endpoint);
        public Uri AddQuery(Uri url, string query) => new Uri(url.ToString() + "?" + query);
        public Uri AppendRoute(Uri url, params string[] routeParts) => new Uri(url.ToString() + "/" + string.Join("/", routeParts));
    }

    public enum ResponseStatusCategory
    {
        Status2xx = 200,
        Status4xx = 400,
        Status5xx = 500
    }
}
