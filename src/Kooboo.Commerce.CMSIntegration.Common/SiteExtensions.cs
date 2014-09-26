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
        public static string GetCommerceInstanceName(this Site site)
        {
            return site.CustomFields["CommerceInstance"];
        }

        public static ICommerceApi Commerce(this Site site)
        {
            var context = new ApiContext(site.GetCommerceInstanceName(), CultureInfo.GetCultureInfo(site.Culture));

            var user = new HttpContextWrapper(HttpContext.Current).Membership().GetMembershipUser();
            if (user != null)
            {
                context.Customer = new CustomerIdentity
                {
                    Email = user.UUID
                };
            }

            var apiType = "Local";
            if (site.CustomFields.ContainsKey("CommerceApiType"))
            {
                apiType = site.CustomFields["CommerceApiType"];
            }

            return ApiService.Get(apiType, context);
        }
    }
}