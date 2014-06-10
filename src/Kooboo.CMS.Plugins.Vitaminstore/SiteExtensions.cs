using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Sites.Models;
using Kooboo.Commerce.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Plugins.Vitaminstore
{
    static class SiteExtensions
    {
        static string GetCommerceName(this Site site)
        {
            // the site's related commerce instance name should be saved in the custom fields by name of "CommerceInstance"
            return GetRequiredCustomField(site, "CommerceInstance");
        }

        static string GetRequiredCustomField(this Site site, string key)
        {
            if (site.CustomFields == null || !site.CustomFields.ContainsKey(key) || string.IsNullOrEmpty(site.CustomFields[key]))
            {
                throw new KeyNotFoundException("To use commerce, please set '" + key + "' in the site's custom fields.");
            }

            return site.CustomFields[key];
        }

        static string GetLanguage(this Site site)
        {
            return site.Culture;
        }

        public static string GetCurrency(this Site site)
        {
            return null;
        }

        public static ICommerceAPI Commerce(this Site site)
        {
            var commerceService = EngineContext.Current.Resolve<ICommerceAPI>();
            commerceService.InitCommerceInstance(site.GetCommerceName(), site.GetLanguage(), site.GetCurrency(), site.CustomFields);
            return commerceService;
        }
    }
}
