using System;
using System.IO;
using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.Server;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Infra.BackgroundServices.Configurations
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }

    public class HangfireActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public HangfireActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type type)
        {
            return _serviceProvider.GetService(type);
        }
    }

    internal class PerformContextValue : LogEventPropertyValue
    {
        // The context attached to this property value
        public PerformContext PerformContext { get; set; }

        /// <inheritdoc />
        public override void Render(TextWriter output, string format = null, IFormatProvider formatProvider = null)
        {
            // How the value will be rendered in Json output, etc.
            // Not important for the function of this code..
            output.Write(PerformContext.BackgroundJob.Id);
        }
    }

    internal class HangfireConsoleSerilogEnricher : ILogEventEnricher
    {
        // The context used to enrich log events
        public PerformContext PerformContext { get; set; }

        /// <inheritdoc />
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            // Create property value with PerformContext and put as "PerformContext"
            var prop = new LogEventProperty("PerformContext", new PerformContextValue
            {
                PerformContext = PerformContext
            });

            logEvent.AddOrUpdateProperty(prop);
        }
    }

    public class HangfireConsoleSink : ILogEventSink
    {
        /// <inheritdoc />
        public void Emit(LogEvent logEvent)
        {
            // Get property
            if (logEvent.Properties.TryGetValue("PerformContext", out var logEventPerformContext))
            {
                // Get the object reference from our custom property
                var performContext = (logEventPerformContext as PerformContextValue)?.PerformContext;
                // And write the line on it
                performContext?.WriteLine(GetColor(logEvent.Level), logEvent.RenderMessage());
            }

            // Some nice coloring for log levels
            static ConsoleTextColor GetColor(LogEventLevel level)
            {
                return level switch
                {
                    LogEventLevel.Fatal => ConsoleTextColor.Red,
                    LogEventLevel.Error => ConsoleTextColor.Red,
                    LogEventLevel.Warning => ConsoleTextColor.Yellow,
                    LogEventLevel.Information => ConsoleTextColor.White,
                    LogEventLevel.Verbose => ConsoleTextColor.Gray,
                    LogEventLevel.Debug => ConsoleTextColor.Gray,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }

    public static class HangfireConsoleSinkExtensions
    {
        public static ILogger CreateLoggerForPerformContext<T>(this PerformContext context)
        {
            return Log.ForContext<T>().ForContext(new HangfireConsoleSerilogEnricher { PerformContext = context });
        }
    }
}