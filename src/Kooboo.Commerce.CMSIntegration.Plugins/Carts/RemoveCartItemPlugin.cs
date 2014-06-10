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
        protected override SubmissionExecuteResult Execute(RemoveItemModel model)
        {
            var cartId = HttpContext.EnsureCart();
            Site.Commerce().ShoppingCarts.RemoveItem(cartId, model.ItemId);

            return null;
        }
    }
}
