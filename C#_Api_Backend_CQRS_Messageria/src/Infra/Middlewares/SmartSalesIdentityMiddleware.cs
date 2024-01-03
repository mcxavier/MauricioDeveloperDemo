using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Infra.Extensions;
using Infra.ExternalServices.Authentication;
using Microsoft.Extensions.Logging;
using Utils.Extensions;
using Infra.EntitityConfigurations.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Middlewares
{
    public class SmartSalesIdentityMiddleware
    {
        private readonly ILogger<SmartSalesIdentityMiddleware> _logger;
        private readonly RequestDelegate _next;
        private const string key = "_smartsales";

        public SmartSalesIdentityMiddleware(RequestDelegate next,
            ILogger<SmartSalesIdentityMiddleware> logger)
        {
            this._logger = logger;
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var identity = context.RequestServices.GetRequiredService<SmartSalesIdentity>();
                var _identityContext = context.RequestServices.GetRequiredService<IdentityContext>();

                var storePortal = context.Request.Headers.TryGet("store");
                if (!storePortal.IsNullOrEmpty() && storePortal.CompareTo("undefined") != 0)
                {
                    var storeEntity = await _identityContext.Stores.FirstOrDefaultAsync(x => x.PortalUrl == storePortal);
                    if (storeEntity == null)
                    {
                        throw new Exception("Loja não encontrada");
                    }
                    identity.CurrentStorePortal = storePortal;
                    identity.CurrentStoreCode = storeEntity.StoreCode;
                    identity.CurrentStoreName = storeEntity.Name;
                    identity.CurrentStoreId = storeEntity.Id;
                    identity.IsCustomer = false;
                    identity.AuthenticationType = "Customer";
                    identity.IsAuthenticated = true;
                    identity.IsCustomer = true;
                    identity.CurrentCompany = storeEntity.CompanyId;
                }

                var tokeHash = context.Request.Headers.TryGet("token");
                if (!tokeHash.IsNullOrEmpty())
                {
                    var linxInfo = JsonExtensions.GetDataFromHash<LinxInfo>(tokeHash);

                    identity.CurrentCompany = linxInfo.Headers.HeadersOperacional.CurrentCompany;
                    var companyEntity = await _identityContext.Companies.FirstOrDefaultAsync(x => x.Id == identity.CurrentCompany);
                    if (companyEntity == null) { throw new Exception("Companhia não encontrada"); }
                    identity.CurrentCompanyName = companyEntity.FullName;
                    identity.Name = string.IsNullOrEmpty(linxInfo.NomeUsuario) ? "Vendedor" : linxInfo.NomeUsuario;
                    identity.IsCustomer = false;
                    identity.AuthenticationType = "Operator";
                    identity.IsAuthenticated = true;
                    context.SetCookie(key, tokeHash);
                    await _next(context);
                    return;
                }

                var cookieUxHash = context.Request.Cookies.TryGet(key);
                if (!cookieUxHash.IsNullOrEmpty())
                {
                    var linxInfo = JsonExtensions.GetDataFromHash<LinxInfo>(cookieUxHash);

                    identity.CurrentCompany = linxInfo.Headers.HeadersOperacional.CurrentCompany;
                    identity.Name = string.IsNullOrEmpty(linxInfo.NomeUsuario) ? "Vendedor" : linxInfo.NomeUsuario;
                    identity.IsCustomer = false;
                    identity.AuthenticationType = "Operator";
                    identity.IsAuthenticated = true;
                    context.SetCookie(key, cookieUxHash);
                    await _next(context);
                    return;
                }

                if (context.Request.Path.Value.Contains("index.html") && context.Request.Path.Value.Contains("swagger"))
                {
                    throw new UnauthorizedAccessException("Cannot Access - User or Tenant or Store not found.");
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
