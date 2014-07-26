using Kooboo.Commerce.Api.Countries;
using Kooboo.Commerce.Api.Customers;
using Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts
{
    public class ChangeShippingAddressPlugin : SubmissionPluginBase<ChangeShippingAddressModel>
    {
        protected override SubmissionExecuteResult Execute(ChangeShippingAddressModel model)
        {
            var cartId = HttpContext.CurrentCartId();
            Api.ShoppingCarts.ChangeShippingAddress(cartId, new Address { Id = model.ShippingAddressId });

            return new SubmissionExecuteResult
            {
                RedirectUrl = ResolveUrl(model.SuccessUrl, ControllerContext)
            };
        }
    }
}
