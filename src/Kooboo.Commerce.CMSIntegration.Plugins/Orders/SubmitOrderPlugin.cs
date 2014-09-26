using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Membership;
using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Payments;
using Kooboo.Commerce.Api.Carts;
using Kooboo.Commerce.CMSIntegration.Plugins.Orders.Models;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Orders
{
    public class SubmitOrderPlugin : SubmissionPluginBase<SubmitOrderModel>
    {
        public SubmitOrderPlugin()
        {
            Parameters["ExpireCart"] = "true";
        }

        protected override SubmissionExecuteResult Execute(SubmitOrderModel model)
        {
            var api = Site.Commerce();

            var member = HttpContext.Membership().GetMembershipUser();
            var cart = api.ShoppingCarts.Query().ByCustomerEmail(member.Email).FirstOrDefault();
            var orderId = api.Orders.CreateFromCart(cart.Id, new ShoppingContext
            {
                Culture = Site.Culture
            });

            if (model.ExpireCart)
            {
                api.ShoppingCarts.ExpireCart(cart.Id);
            }

            return new SubmissionExecuteResult
            {
                RedirectUrl = ResolveUrl(model.ReturnUrl, ControllerContext),
                Data = new SubmitOrderResult
                {
                    OrderId = orderId
                }
            };
        }
    }
}
