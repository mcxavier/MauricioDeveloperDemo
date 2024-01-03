using System;
using Infra.BackgroundServices.Configurations;
using Infra.Module;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Email;

namespace Api.App_Infra
{
    public class LoggerConfigurations
    {
        public static readonly string Namespace = typeof(InfraModule).Namespace;

        public static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var logstashUrl = configuration["Serilog:LogstashgUrl"];

            return new LoggerConfiguration()
                   .ReadFrom.Configuration(configuration)
                       .Enrich.FromLogContext()
                       .Enrich.WithMachineName()
                       .Enrich.WithEnvironmentUserName()
                       .Enrich.WithProcessId()
                       .Enrich.WithThreadId()
                       .Enrich.WithExceptionDetails()
                       .Enrich.WithProperty("ApplicationContext", Namespace)
                   .MinimumLevel.Verbose()
                   .WriteTo.Sink(new HangfireConsoleSink())
                   .MinimumLevel.Information()
                   .WriteTo.Console()                  
                   .WriteTo.Email(
                       restrictedToMinimumLevel: LogEventLevel.Fatal,
                       period: TimeSpan.FromSeconds(1),
                       connectionInfo: new EmailConnectionInfo
                       {
                           //MailServer = smtp.Host, 
                           //NetworkCredentials = new NetworkCredential(smtp.Username, smtp.Password),
                           //FromEmail = smtp.Username,
                           //EnableSsl = smtp.Ssl,
                           EmailSubject = "SMSL - Error [{Message:lj}]",
                           IsBodyHtml = true,
                           //Port = smtp.Port,
                           ToEmail = @"joao.martins@terceiroslinx.com.br;jean.prates@linx.com.br"
                       }).CreateLogger();
        }
    }

}