using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.Commerce.API
{
    public static class KoobooSiteExtensions
    {
        public static string GetCommerceName(this Site site)
        {
            if (site.CustomFields != null && site.CustomFields.ContainsKey("CommerceInstance"))
            {
                return site.CustomFields["CommerceInstance"];
            }
            throw new ApplicationException("CommerceInstance is not found in site custom fields.");
        }

        public static string GetLanguage(this Site site)
        {
            return site.Culture;
        }

        public static ICommerceService Commerce(this Site site)
        {
            return EngineContext.Current.Resolve<ICommerceService>();
        }
    }
}
