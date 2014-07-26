using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Kooboo.CMS.Sites.Membership;
using Kooboo.Commerce.CMSIntegration;

namespace Kooboo.CMS.Plugins.Vitaminstore
{
    public static class HttpContextExtensions
    {
        public static string CurrentSessionId(this HttpContextBase context)
        {
            return context.Session.SessionID;
        }

        public static string CurrentCustomerAccountId(this HttpContextBase context)
        {
            var member = context.Membership().GetMembershipUser();
            return member == null ? null : member.UUID;
        }

        public static int CurrentCartId(this HttpContextBase context)
        {
            var accountId = context.CurrentCustomerAccountId();
            if (!String.IsNullOrWhiteSpace(accountId))
            {
                return Site.Current.Commerce().ShoppingCarts.CustomerCartId(accountId);
            }

            return Site.Current.Commerce().ShoppingCarts.SessionCartId(context.CurrentSessionId());
        }
    }
}
