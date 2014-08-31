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
        protected override SubmissionExecuteResult Execute(ApplyCouponModel model)
        {
            var cartId = HttpContext.CurrentCartId();
            var success = Api.ShoppingCarts.ApplyCoupon(cartId, model.CouponCode);

            if (!success)
            {
                throw new InvalidModelStateException(new Dictionary<string, string>
                {
                    { "CouponCode", "Invalid coupon code" }
                });
            }

            return new SubmissionExecuteResult
            {
                RedirectUrl = ResolveUrl(model.SuccessUrl, ControllerContext),
                Data = new ApplyCouponResult
                {
                    Applied = success
                }
            };
        }
    }
}
