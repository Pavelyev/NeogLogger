using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
                    Debug.WriteLine($"calling logger {DateTime.Now.ToLongTimeString()}");
                    LoggerHolder.Logger.Flush();
                    await Task.Delay(1000);
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