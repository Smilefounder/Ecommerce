using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.ShoppingCarts.Services;
using Kooboo.CMS.Membership.Models;

namespace Kooboo.Commerce.API.RestAPI
{
    [Dependency(typeof(IOrderAPI), ComponentLifeStyle.Transient, Key = "RestAPI")]
    public class OrderAPI : RestApiBase, IOrderAPI
    {
        public Order GetMyOrder(string sessionId, MembershipUser user, bool expireShoppingCart = true)
        {
            QueryParameters.Add("sessionId", sessionId);
            QueryParameters.Add("expireShoppingCart", expireShoppingCart.ToString());
            return Post<Order>(null, user);
        }

        public bool SaveOrder(Order order)
        {
            return Put<bool>(null, order);
        }

        protected override string ApiControllerPath
        {
            get { return "Order"; }
        }
    }
}
