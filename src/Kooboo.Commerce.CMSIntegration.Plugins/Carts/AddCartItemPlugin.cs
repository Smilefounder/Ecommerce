using Kooboo.CMS.Sites.Models;
using Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Membership;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts
{
    public class AddCartItemPlugin : SubmissionPluginBase<AddItemModel>
    {
        protected override object Execute(AddItemModel model)
        {
            var sessionId = HttpContext.Session.SessionID;
            var member = HttpContext.Membership().GetMembershipUser();
            var accountId = member == null ? null : member.UUID;
            Site.Commerce().ShoppingCarts.AddToCart(sessionId, accountId, model.ProductPriceId, model.Quantity);

            return null;
        }
    }
}
