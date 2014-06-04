using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Payments.iDeal
{
    public static class UrlStringExtensions
    {
        public static string ToFullUrl(this string url, HttpContextBase httpContext)
        {
            Require.NotNullOrEmpty(url, "url");
            Require.NotNull(httpContext, "httpContext");

            if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                return url;
            }

            var request = httpContext.Request;
            var baseUrl = request.Url.Scheme + "://" + request.Url.Authority;

            return UrlUtility.Combine(baseUrl, url);
        }
    }
}
