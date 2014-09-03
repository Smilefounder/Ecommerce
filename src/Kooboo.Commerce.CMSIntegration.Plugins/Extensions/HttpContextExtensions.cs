using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Kooboo.CMS.Sites.Membership;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.Commerce.CMSIntegration.Plugins
{
    static class HttpContextExtensions
    {
        public static string CurrentCustomerEmail(this HttpContextBase context)
        {
            var member = context.Membership().GetMembershipUser();
            return member == null ? null : member.Email;
        }

        public static int CurrentCartId(this HttpContextBase context)
        {
            var email = context.CurrentCustomerEmail();
            if (!String.IsNullOrWhiteSpace(email))
            {
                return Site.Current.Commerce().ShoppingCarts.GetCartIdByCustomer(email);
            }

            var sessionId = EngineContext.Current.Resolve<IShoppingCartSessionIdProvider>().GetCurrentSessionId(true);
            return Site.Current.Commerce().ShoppingCarts.GetCartIdBySessionId(sessionId);
        }
    }
}
