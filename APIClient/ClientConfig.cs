using APIClient.Auth;
using APIClient.Serializers;
using System.Text;

namespace APIClient
{
    public class ClientConfig
    {
        public Protocol Protocol;
        public string ApiPath;
        public IAuthModule AuthModule;
        public ISerializer Serializer;
        public RequestBuilder RequestBuilder;
        public ResponseStatusCategory[] AcceptedResponseStatuses;

        public static ClientConfig GetBasicConfig(Protocol protocol, string apiPath) => new ClientConfig()
            {
                Protocol = protocol,
                ApiPath = apiPath, 
                AuthModule = new NoAuthModule(), 
                Serializer = new JsonSerializer(true), 
                RequestBuilder = new RequestBuilder("application/json", Encoding.UTF8),
                AcceptedResponseStatuses = new [] { ResponseStatusCategory.Status2xx }
            };

        public static ClientConfig GetConfigIgnoring4xxAndDeserializerError(Protocol protocol, string apiPath)
        {
            var config = GetBasicConfig(protocol, apiPath);
            config.Serializer = new JsonSerializer(false);
            config.AcceptedResponseStatuses = new [] { ResponseStatusCategory.Status2xx, ResponseStatusCategory.Status4xx };
            return config;
        }

        public static ClientConfig GetConfigIgnoringStatusAndDeserializerError(Protocol protocol, string apiPath)
        {
            var config = GetBasicConfig(protocol, apiPath);
            config.Serializer = new JsonSerializer(false);
            config.AcceptedResponseStatuses = new [] { ResponseStatusCategory.Status2xx, ResponseStatusCategory.Status4xx, ResponseStatusCategory.Status5xx };
            return config;
        }

        public static ClientConfig GetConfigIgnoringMissingRequiredFields(Protocol protocol, string apiPath)
        {
            var config = GetBasicConfig(protocol, apiPath);
            config.Serializer = new JsonSerializer(true, JsonSerializerSettingsFactory.GetSerializerIgnoringMissingRequiredFields());
            return config;
        }
    }
}
