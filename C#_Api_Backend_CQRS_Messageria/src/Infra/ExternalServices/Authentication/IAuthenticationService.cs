using Infra.ExternalServices.Authentication.Dtos;

using System.Threading.Tasks;

namespace Infra.ExternalServices.Authentication
{

    public interface IAuthenticationService
    {

        Task<AuthenticationDto> AuthenticateAsync(string username, string password);

    }

}