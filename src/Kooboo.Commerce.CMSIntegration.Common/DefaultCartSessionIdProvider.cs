using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration
{
    [Dependency(typeof(ICartSessionIdProvider))]
    public class DefaultCartSessionIdProvider : ICartSessionIdProvider
    {
        public string CookieName { get; set; }

        public TimeSpan CookieLifetime { get; set; }

        public Func<HttpContextBase> GetHttpContext = () => new HttpContextWrapper(HttpContext.Current);

        public DefaultCartSessionIdProvider()
        {
            CookieName = "cart.sessionid";
            CookieLifetime = TimeSpan.FromDays(60);
        }

        public string GetCurrentSessionId(bool ensure)
        {
            var httpContext = GetHttpContext();
            var cookie = httpContext.Request.Cookies[CookieName];

            if (cookie != null && !String.IsNullOrWhiteSpace(cookie.Value))
            {
                if (ensure)
                {
                    TryUpdateCookieExpireTime(cookie);
                }

                return cookie.Value;
            }

            if (ensure)
            {
                var sessionId = NewSessionId();

                cookie = new HttpCookie(CookieName, sessionId);
                TryUpdateCookieExpireTime(cookie);

                httpContext.Response.AppendCookie(cookie);

                return sessionId;
            }

            return null;
        }

        private void TryUpdateCookieExpireTime(HttpCookie cookie)
        {
            if (CookieLifetime != TimeSpan.Zero)
            {
                cookie.Expires = DateTime.Now.Add(CookieLifetime);
            }
        }

        private string NewSessionId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
