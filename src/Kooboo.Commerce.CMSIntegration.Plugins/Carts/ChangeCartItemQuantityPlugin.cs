using Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Membership;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts
{
    public class ChangeCartItemQuantityPlugin : SubmissionPluginBase<ChangeCartItemQuantityModel>
    {
        protected override SubmissionExecuteResult Execute(ChangeCartItemQuantityModel model)
        {
            var cartId = HttpContext.CurrentCartId();
            Api.ShoppingCarts.ChangeItemQuantity(cartId, model.ItemId, model.NewQuantity);

            return new SubmissionExecuteResult
            {
                RedirectUrl = ResolveUrl(model.SuccessUrl, ControllerContext)
            };
        }
    }
}
