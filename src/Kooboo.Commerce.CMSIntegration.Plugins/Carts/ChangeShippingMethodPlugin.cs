using Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts
{
    public class ChangeShippingMethodPlugin : SubmissionPluginBase<ChangeShippingMethodModel>
    {
        protected override SubmissionExecuteResult Execute(ChangeShippingMethodModel model)
        {
            var cartId = HttpContext.CurrentCartId();
            Api.ShoppingCarts.ChangeShippingMethod(cartId, model.ShippingMethodId);

            return new SubmissionExecuteResult
            {
                RedirectUrl = ResolveUrl(model.SuccessUrl, ControllerContext)
            };
        }
    }
}
