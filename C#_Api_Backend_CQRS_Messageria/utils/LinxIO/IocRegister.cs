using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using LinxIO.Interfaces;
using LinxIO.Dtos;

namespace LinxIO
{
    public static class IocRegister
    {
        public static void AddLinxIO(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ILinxIOClientConfig>(x => x.GetRequiredService<IOptions<LinxIOClientConfig>>().Value);            
        }
    }
}