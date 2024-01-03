namespace Infra.ExternalServices.Authentication.Dtos
{

    public class AuthenticationDto
    {

        public string CurrentCompany { get; set; }

        public string AuthorizationToken { get; set; }

        public string AccessGroup { get; set; } = "00000000-0000-0000-0000-000000000000";

        public string CurrentUser { get; set; }

        public string EconomicGroup { get; set; }

        public string Environment { get; set; }

        public string Application { get; set; }

        public string UserId { get; set; }

        public string ServiceBusUrl { get; set; }

    }

}