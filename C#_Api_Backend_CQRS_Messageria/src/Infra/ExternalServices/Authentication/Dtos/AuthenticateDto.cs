using System.Collections.Generic;

namespace Infra.ExternalServices.Authentication.Dtos
{

    public class AuthenticateJsonResult
    {

        public int Key { get; set; }

        public string Value { get; set; }

    }

    public class AuthenticateDto
    {

        public List<AuthenticateJsonResult> AuthenticateJsonResult { get; set; }

    }

}