using Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts
{
    public class ApplyCouponPlugin : SubmissionPluginBase<ApplyCouponModel>
    {
        protected override object Execute(ApplyCouponModel model)
        {
            var cart = Site.GetCurrentCart(ControllerContext);
            if (cart != null)
            {
                if (!Site.Commerce().ShoppingCarts.ApplyCoupon(cart.Id, model.Coupon))
                {
                    throw new Exception("Invalid coupon code.");
                }
            }

            return null;
        }
    }
}
