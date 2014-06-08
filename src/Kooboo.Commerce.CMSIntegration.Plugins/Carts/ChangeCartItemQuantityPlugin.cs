using Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Membership;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts
{
    public class ChangeCartItemQuantityPlugin : SubmissionPluginBase<ChangeItemQuantityModel>
    {
        protected override object Execute(ChangeItemQuantityModel model)
        {
            var sessionId = HttpContext.Session.SessionID;
            var member = HttpContext.Membership().GetMembershipUser();
            Site.Commerce().ShoppingCarts.UpdateCart(sessionId, member == null ? null : member.UUID, model.ProductPriceId, model.NewQuantity);

            return null;
        }
    }
}
