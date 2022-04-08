using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalR.Service.Manager;
using System;
using System.Threading.Tasks;

namespace SignalR.Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            BuildHost(args).Build().Run();
        }

        public static IHostBuilder BuildHost(string[] args)
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var host = Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration(builder => builder.SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                        .AddEnvironmentVariables())
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });

#if (!DEBUG)
                    .UseWindowsService();
#endif
            return host;
        }
            
    }
}
