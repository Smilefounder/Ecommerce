using Kooboo.Commerce.API.Locations;
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
            var cart = Site.GetCurrentCart(ControllerContext);
            Site.Commerce().ShoppingCarts.ChangeShippingAddress(cart.Id, new Address { Id = model.NewShippingAddressId });

            return null;
        }
    }
}
