using Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts
{
    public class RemoveCartItemPlugin : SubmissionPluginBase<RemoveItemModel>
    {
        protected override object Execute(RemoveItemModel model)
        {
            var cart = Site.GetCurrentCart(ControllerContext);
            if (cart != null)
            {
                Site.Commerce().ShoppingCarts.RemoveCartItem(cart.Id, model.ItemId);
            }

            return null;
        }
    }
}
