using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Membership;
using Kooboo.Commerce.Api;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration
{
    public static class SiteExtensions
    {
        public static string ApiType(this Site site)
        {
            if (site.CustomFields.ContainsKey("CommerceApiType"))
            {
                return site.CustomFields["CommerceApiType"];
            }

            return "Local";
        }

        public static string GetCommerceInstanceName(this Site site)
        {
            return site.CustomFields["CommerceInstance"];
        }

        public static string GetCurrency(this Site site)
        {
            return null;
        }

        public static ICommerceApi Commerce(this Site site)
        {
            var user = new HttpContextWrapper(HttpContext.Current).Membership().GetMembershipUser();
            var accountId = user == null ? null : user.UUID;

            var context = new ApiContext(site.GetCommerceInstanceName(), CultureInfo.GetCultureInfo(site.Culture), site.GetCurrency())
            {
                CustomerEmail = accountId
            };

            var apiType = "Local";
            if (site.CustomFields.ContainsKey("CommerceApiType"))
            {
                apiType = site.CustomFields["CommerceApiType"];
            }

            return ApiService.Get(apiType, context);
        }
    }
}