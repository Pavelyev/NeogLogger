using System;
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
                return Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        try
                        {
                            client.LogDeferredRandomMessage().Wait();
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine($"error in webapi\n{exception}");
                        }
                    }
                });
            });
            Task.WaitAll(tasks.ToArray());
        }
    }
}