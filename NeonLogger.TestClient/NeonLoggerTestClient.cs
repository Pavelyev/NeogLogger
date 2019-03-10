using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NeonLogger.TestClient
{
    public class NeonLoggerTestClient
    {
        public async Task<HttpResponseMessage> LogRandomMessage()
        {
            return await _client.Log(RandomMessage());
        }

        public async Task<HttpResponseMessage> LogDeferredRandomMessage()
        {
            return await _client.LogDeferred(RandomMessage());
        }

        private string RandomMessage()
        {
            return $"Message {_random.Next(0, 100):00}";
        }

        private readonly Random _random = new Random();
        private readonly NeonLoggerWebClient _client;

        public NeonLoggerTestClient(string url)
        {
            _client = new NeonLoggerWebClient(url);
        }
    }
}