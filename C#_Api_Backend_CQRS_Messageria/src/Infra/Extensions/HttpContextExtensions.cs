using System;
using Microsoft.AspNetCore.Http;

namespace Infra.Extensions
{
    public static class HttpContextExtensions
    {
        public static void SetCookie(this HttpContext context, string key, string value)
        {
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(7);
            context.Response.Cookies.Append(key, value, cookieOptions);
        }

        public static string TryGet(this IHeaderDictionary source, string term)
        {
            try
            {
                return source[term];
            }
            catch
            {
                return "";
            }
        }

        public static string TryGet(this IRequestCookieCollection source, string term)
        {
            try
            {
                return source[term];
            }
            catch
            {
                return "";
            }
        }
    }
}