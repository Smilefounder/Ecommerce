using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations
{
    public static class HttpContextVisitorCookieExtensions
    {
        const string CookieKey = "__uniqueid";

        public static string GetVisitorUniqueId(this HttpContextBase context)
        {
            var cookie = context.Request.Cookies[CookieKey];
            return cookie == null ? null : cookie.Value;
        }

        public static string EnsureVisitorUniqueId(this HttpContextBase context)
        {
            var cookie = context.Request.Cookies[CookieKey];
            if (cookie == null)
            {
                cookie = new HttpCookie(CookieKey, Guid.NewGuid().ToString("N"));
                cookie.HttpOnly = true;
                context.Response.Cookies.Add(cookie);
            }
            else
            {
                cookie.Expires = DateTime.Now.AddYears(3);
            }

            return cookie.Value;
        }
    }
}