using System.Linq;
using System.Threading.Tasks;

namespace NeonLogger.TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var clients = Enumerable.Range(1, 4)
                .Select(x => new NeonLoggerTestClient("http://localhost:5000"));
            var tasks = clients.Select(client =>
            {
                return Task.Factory.StartNew(x =>
                {
                    while (true)
                    {
                        client.LogDeferredRandomMessage().Wait();
                    }
                }, TaskCreationOptions.LongRunning);
            });
            Task.WaitAll(tasks.ToArray());
        }
    }
}