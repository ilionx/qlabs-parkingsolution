using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure;
using Microsoft.Extensions.Logging;

namespace ProjectParking.WebApps.ParkingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                .UseApplicationInsights()
                .ConfigureLogging(cfg => { cfg.AddConsole().AddDebug(); })
                .UseStartup<Startup>()
                .Build();
        }
    }
}