using Api.App_Infra;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Module
{
    public static class AppModule
    {
        public static void AddAppModule(this IServiceCollection services)
        {
            services.AddScoped<SmartSalesAuthorizeAttribute>();

            services.AddControllers().AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                        options.JsonSerializerOptions.IgnoreNullValues = true;
                    });

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithOrigins(
                              "http://localhost:3000",
                              "https://localhost:3000",
                              "https://dev-smartsales.com.br",
                              "https://qa-smartsales.com.br",
                              "https://hml-smartsales.com.br",
                              "https://smartsales.linx.com.br",
                              "https://smartsales.com.br",
                              "https://www.dev-smartsales.com.br",
                              "https://www.qa-smartsales.com.br",
                              "https://www.hml-smartsales.com.br",
                              "https://www.smartsales.com.br"
                          )
                          .AllowCredentials());
            });
        }
    }
}