using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NeonLogger.TestClient
{
    public class NeonLoggerWebClient
    {
        public async Task<HttpResponseMessage> Log(string message)
        {
            var data = new {message};
            var content = ToJsonContent(data);
            return await _httpClient.PostAsync(_url + "/Logger/Log", content);
        }

        public async Task<HttpResponseMessage> LogDeferred(string message)
        {
            var data = new {message};
            var content = ToJsonContent(data);
            return await _httpClient.PostAsync(_url + "/Logger/LogDeferred", content);
        }

        private StringContent ToJsonContent(object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _url;

        public NeonLoggerWebClient(string url)
        {
            _url = url;
        }
    }
}