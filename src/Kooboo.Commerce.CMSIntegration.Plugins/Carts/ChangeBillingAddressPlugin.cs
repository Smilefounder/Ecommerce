using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts
{
    public class ChangeBillingAddressPlugin : SubmissionPluginBase<ChangeBillingAddressModel>
    {
        protected override SubmissionExecuteResult Execute(ChangeBillingAddressModel model)
        {
            var cartId = HttpContext.CurrentCartId();
            Api.ShoppingCarts.ChangeBillingAddress(cartId, new Address { Id = model.NewBillingAddressId });

            return null;
        }
    }
}
