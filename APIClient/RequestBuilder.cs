using System;
using System.Net.Http;
using System.Text;

namespace APIClient
{
    public class RequestBuilder
    {
        private string _mediaType;
        private Encoding _encoding;
        public RequestBuilder(string mediaType, Encoding encoding)
        {
            _mediaType = mediaType;
            _encoding = encoding;
        }

        public HttpRequestMessage BuildRequest(HttpMethod method, Uri uri) => new HttpRequestMessage() { Method = method, RequestUri = uri };

        public HttpRequestMessage BuildRequest(HttpMethod method, Uri uri, string content) => 
            new HttpRequestMessage() { Method = method, RequestUri = uri, Content = new StringContent(content, _encoding, _mediaType)};

    }
}