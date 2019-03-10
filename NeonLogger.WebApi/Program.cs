using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace NeonLogger.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    LoggerHolder.Logger.Flush();
                    await Task.Delay(100);
                }
            });
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

    public static class LoggerHolder
    {
        public static readonly NeonLogger Logger = new NeonLogger("log.txt");
    }
}