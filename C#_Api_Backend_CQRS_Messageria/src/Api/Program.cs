using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

namespace Api
{
    public class Program
    {

        public static readonly string Namespace = typeof(Program).Namespace;

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            try
            {
                var host = CreateHostBuilder(args);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost CreateHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .CaptureStartupErrors(false)
                          .ConfigureAppConfiguration((hostContext, config) =>
                          {
                              var env = hostContext.HostingEnvironment;

                              config.Sources.Clear();
                              config.AddJsonFile("appsettings.json", false, true);
                              config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
                              config.AddEnvironmentVariables();
                          })
                          .UseStartup<Startup>()
                          .UseUrls("http://0.0.0.0:5000/", "https://0.0.0.0:5001/")
                          .UseSerilog()
                          .UseContentRoot(Directory.GetCurrentDirectory())
                          .Build();
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                   .ReadFrom.Configuration(configuration)
                   .MinimumLevel.Information()
                   .WriteTo.ApplicationInsights(new TelemetryConfiguration { InstrumentationKey = configuration["InstrumentationKey"] }, TelemetryConverter.Traces)
                   .WriteTo.Console(outputTemplate: "<{Level:u3} {Timestamp:yyyy-M-d HH:mm:ss}> {Message:lj} {NewLine} {Exception}")
                   .Enrich.WithProperty("ApplicationContext", Namespace)
                   .Enrich.FromLogContext()
                   .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}