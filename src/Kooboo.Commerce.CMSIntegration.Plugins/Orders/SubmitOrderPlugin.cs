using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Membership;
using Kooboo.Commerce.API.Payments;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Orders
{
    public class SubmitOrderPlugin : SubmissionPluginBase
    {
        protected override SubmissionExecuteResult Execute(SubmissionModel model)
        {
            var api = Site.Commerce();

            var member = HttpContext.Membership().GetMembershipUser();
            var cart = api.ShoppingCarts.ByAccountId(member.UUID).FirstOrDefault();
            var order = api.Orders.CreateFromCart(cart.Id, member, true);

            api.ShoppingCarts.ExpireCart(cart.Id);

            return new SubmissionExecuteResult
            {
                Data = new SubmitOrderResult
                {
                    OrderId = order.Id
                }
            };
        }
    }
}
