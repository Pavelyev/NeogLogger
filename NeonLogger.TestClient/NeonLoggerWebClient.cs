using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NeonLogger.TestClient
{
    public class NeonLoggerWebClient
    {
        public async Task<HttpResponseMessage> LogRandomMessage()
        {
            var data = new {message = RandomMessage()};
            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await _httpClient.PostAsync(_url + "/Logger/Log", content);
        }

        public async Task<HttpResponseMessage> LogDeferredRandomMessage()
        {
            var data = new {message = RandomMessage()};
            Console.WriteLine(data);
            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await _httpClient.PostAsync(_url + "/Logger/LogDeferred", content);
        }

        private string RandomMessage()
        {
            return $"Message {_random.Next(0, 100):00}";
        }

        private readonly Random _random = new Random();
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _url;

        public NeonLoggerWebClient(string url)
        {
            _url = url;
        }
    }
}