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
        protected override SubmissionExecuteResult Execute(ChangeItemQuantityModel model)
        {
            var cartId = HttpContext.CurrentCartId();
            Api.ShoppingCarts.ChangeItemQuantity(cartId, model.ItemId, model.NewQuantity);

            return null;
        }
    }
}
