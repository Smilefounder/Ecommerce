using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
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

        public static string CommerceInstanceName(this Site site)
        {
            return site.CustomFields["CommerceInstance"];
        }
    }
}