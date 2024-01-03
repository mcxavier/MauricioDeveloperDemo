using System;
using System.Threading.Tasks;
using Infra.ExternalServices.Reshop.Dtos;

namespace CoreService.Infrastructure.Services
{
    public interface ILgpdIntegrationService
    {
        Task<CustomerPermission> GetPermission(String document);
    }
}