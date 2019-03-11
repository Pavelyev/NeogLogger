using System;
using System.IO;
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
                    try
                    {
                        LoggerHolder.Logger.Flush();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"logger can't Flush\n{exception}");
                    }

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
        public static readonly NeonLogger Logger = new NeonLogger(Path.Combine("Log", "log.txt"));
    }
}