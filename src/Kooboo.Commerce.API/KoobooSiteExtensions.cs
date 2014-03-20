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

        public static string GetLanguage(this Site site)
        {
            return site.Culture;
        }

        public static ICommerceAPI Commerce(this Site site)
        {
            //if(site.CustomFields == null || !site.CustomFields.ContainsKey("CommerceAPI") || string.IsNullOrEmpty(site.CustomFields["CommerceAPI"]))
            //{
            //    throw new Exception("To use commerce, please set 'CommerceAPI' in the site's custom fields.");
            //}
            //string api = site.CustomFields["CommerceAPI"];
            //var commerceService = EngineContext.Current.Resolve<ICommerceAPI>(api);
            //if(commerceService is RestAPI.RestCommerceAPI)
            //{
            //    if (site.CustomFields == null || !site.CustomFields.ContainsKey("WebAPIHost") || string.IsNullOrEmpty(site.CustomFields["WebAPIHost"]))
            //    {
            //        throw new Exception("To use commerce by web api, please set 'WebAPIHost' in the site's custom fields.");
            //    }
            //    string webapiHost = site.CustomFields["WebAPIHost"];
            //    ((RestAPI.RestCommerceAPI)commerceService).SetWebAPIHost(webapiHost);
            //}
            var commerceService = EngineContext.Current.Resolve<ICommerceAPI>();
            commerceService.InitCommerceInstance(site.GetCommerceName(), site.GetLanguage());
            return commerceService;
        }
    }
}
