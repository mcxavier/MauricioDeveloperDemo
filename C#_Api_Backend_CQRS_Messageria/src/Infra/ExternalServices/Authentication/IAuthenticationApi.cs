namespace Infra.ExternalServices.Authentication
{

    public interface IAuthenticationApi
    {

        string AuthenticateJson(string username, string password);

    }

}