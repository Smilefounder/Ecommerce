using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    public static class SiteExtensions
    {
        public static string CommerceInstanceName(this Site site)
        {
            return site.CustomFields["CommerceInstance"];
        }
    }
}