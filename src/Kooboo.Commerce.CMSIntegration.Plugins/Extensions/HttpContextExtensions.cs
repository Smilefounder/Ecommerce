using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.Plugins
{
    static class HttpContextExtensions
    {
        public static string CurrentCartSessionId(this HttpContextBase context)
        {
            return context.Session.SessionID;
        }

        // TODO: Add an encrypted cart id in the cookie like User.Identity.Name is way more easier
        public static int EnsureCart(this HttpContextBase context)
        {
            var identity = context.User.Identity;
            if (identity.IsAuthenticated)
            {
                return Site.Current.Commerce().ShoppingCarts.EnsureCustomerCart(identity.Name, context.CurrentCartSessionId());
            }

            return Site.Current.Commerce().ShoppingCarts.EnsureSessionCart(context.CurrentCartSessionId());
        }
    }
}
