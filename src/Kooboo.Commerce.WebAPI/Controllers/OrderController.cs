using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.API.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class OrderController : CommerceAPIControllerBase
    {
        public Order Post(string sessionId, bool expireShoppingCart, [FromBody]MembershipUser user)
        {
            return Commerce().Orders.GetMyOrder(sessionId, user, expireShoppingCart);
        }

        public bool Put([FromBody]Order order)
        {
            return Commerce().Orders.Save(order);
        }
    }
}
