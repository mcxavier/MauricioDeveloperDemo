using System;
using System.Threading.Tasks;
using Infra.ExternalServices.Reshop.Dtos;

namespace CoreService.Infrastructure.Services
{
    public interface IReshopIntegrationService
    {
        Task<ReshopToken> GetToken();
    }
}