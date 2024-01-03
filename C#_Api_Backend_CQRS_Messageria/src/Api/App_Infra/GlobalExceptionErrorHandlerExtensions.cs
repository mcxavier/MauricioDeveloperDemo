using System;
using System.Linq;
using System.Net;
using FluentValidation;
using Infra.QueryCommands._Kernel.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Utils.Extensions;
using Core.SharedKernel;

namespace Api.App_Infra
{

    public static class GlobalExceptionErrorHandlerExtensions
    {

        public static void AddGlobalExceptionErrorHandler(this IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var exception = contextFeature.Error;
                    context.Response.StatusCode = (int)GetErrorCode(exception);
                    context.Response.ContentType = "application/json";

                    if (!(exception is UnauthorizedAccessException))
                    {
                        logger.LogCritical(@"<br/><br/>
                           Error: <strong>{message}</strong><br/> 
                           <strong>Host: {host}</strong><br/>
                           <strong>Path: {path}</strong><br/>
                           <strong>Queries: {query}</strong><br/>
                           <strong>Method: {method}</strong><br/>
                           <strong>Headers:</strong> <pre>{headers}</pre><br/>
                           <pre>{@exception}</pre><br/>",
                            exception.Message.Truncate(60),
                            context.Request.Host.Value,
                            context.Request.Path.Value,
                            context.Request.Query.ToArray(),
                            context.Request.Method,
                            context.Request.Headers.Select(x => x.Key + ": " + x.Value.Acumullate("    <br/>")).Acumullate("<br/>"),
                            exception
                        );
                    }

                    var result = JsonConvert.SerializeObject(new Response
                    {
                        IsError = true,
                        Message = exception.Message,
                        Payload = env.IsDevelopment() ? exception : null
                    }, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    if (exception is CommandValidationException validationException)
                    {
                        result = JsonConvert.SerializeObject(new Response
                        {
                            IsError = true,
                            Message = "Erros de validação",
                            Errors = validationException.ValidationErrors.Select(x => new Error
                            {
                                Code = 0,
                                Message = x
                            }).ToList()
                        }, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    }

                    if (exception is UnauthorizedAccessException)
                    {
                        result = JsonConvert.SerializeObject(new Response
                        {
                            IsError = true,
                            Message = "Não autorizado"
                        }, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    }

                    await context.Response.WriteAsync(result);
                }
            }));
        }

        private static HttpStatusCode GetErrorCode(Exception e)
        {
            return e switch
            {
                ValidationException _ => HttpStatusCode.BadRequest,
                FormatException _ => HttpStatusCode.BadRequest,
                UnauthorizedAccessException _ => HttpStatusCode.Unauthorized,
                NotImplementedException _ => HttpStatusCode.NotImplemented,
                CommandValidationException _ => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };
        }
    }

}