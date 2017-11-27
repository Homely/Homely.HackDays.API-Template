using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace API.CRUD
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // SERILOG + Staging/Production builds?
            // Where is the information gathered from.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");

                var host = WebHost.CreateDefaultBuilder(args)
                                  .UseContentRoot(Directory.GetCurrentDirectory())
                                  .UseIISIntegration()
                                  .UseStartup<Startup>()
                                  .UseConfiguration(configuration)
                                  .UseSerilog()
                                  .UseApplicationInsights()
                                  .Build();
                host.Run();

                return 0;
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
