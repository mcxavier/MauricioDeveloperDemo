using System;
using System.Net;
using Infra.ExternalServices.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Core.SharedKernel;

namespace Api.App_Infra
{

    public class SmartSalesAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly SmartSalesIdentity _salesIdentity;

        public SmartSalesAuthorizeAttribute(SmartSalesIdentity identity, ILogger<SmartSalesAuthorizeAttribute> logger)
        {
            _salesIdentity = identity;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_salesIdentity.IsAuthenticated)
                return;

            var result = new JsonResult(new Response
            {
                IsError = true,
                Message = "Não autorizado",
            });

            result.StatusCode = (int)HttpStatusCode.Unauthorized;

            context.Result = result;
        }
    }
}