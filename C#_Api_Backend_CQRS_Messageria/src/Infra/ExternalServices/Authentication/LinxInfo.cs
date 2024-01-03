using System;
using System.Security.Principal;

namespace Infra.ExternalServices.Authentication
{

    public interface ILinxInfo
    { }

    public class LinxInfo : ILinxInfo
    {

        public bool IsCustomerRequest { get; set; }

        public int IdLinxOperacional { get; set; }

        public int IdLinxAdministrativo { get; set; }

        public int IdGpecon { get; set; }

        public int IdUsuario { get; set; }

        public string NomeUsuario { get; set; }

        public string ConnectionString { get; set; }

        public Headers Headers { get; set; } = new Headers();

        public bool IsValid() => (Headers?.HeadersOperacional?.CurrentCompany != null);

        public void SetData(LinxInfo info)
        {
            this.Headers.HeadersOperacional.CurrentCompany = info.Headers.HeadersOperacional.CurrentCompany;
        }

    }

    public class Headers
    {

        public HeadersOperacional HeadersOperacional { get; set; } = new HeadersOperacional();

    }

    public class HeadersOperacional
    {

        public string URL { get; set; }

        public Guid? CurrentCompany { get; set; }

        public Guid? AuthorizationToken { get; set; }

        public Guid? CurrentUser { get; set; }

        public Guid? AccessGroup { get; set; }

        public Guid? EconomicGroup { get; set; }

        public Guid? Application { get; set; }

        public int Environment { get; set; }

    }

}