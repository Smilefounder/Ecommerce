using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.Commerce.API.HAL;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers
{
    public class OrderController : CommerceAPIControllerAccessBase<Order>
    {
        /// <summary>
        /// build the commerce query filters from query string.
        /// </summary>
        /// <returns>commerce query</returns>
        protected override ICommerceQuery<Order> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().Orders.Query();
            if (!string.IsNullOrEmpty(qs["id"]))
                query = query.ById(Convert.ToInt32(qs["id"]));
            if (!string.IsNullOrEmpty(qs["customerId"]))
                query = query.ByCustomerId(Convert.ToInt32(qs["customerId"]));
            if (!string.IsNullOrEmpty(qs["accountId"]))
                query = query.ByAccountId(qs["accountId"]);
            DateTime? fromCreateDate = null, toCreateDate = null;
            if (!string.IsNullOrEmpty(qs["fromCreateDate"]))
                fromCreateDate = Convert.ToDateTime(qs["fromCreateDate"]);
            if (!string.IsNullOrEmpty(qs["toCreateDate"]))
                toCreateDate = Convert.ToDateTime(qs["toCreateDate"]);
            query = query.ByCreateDate(fromCreateDate, toCreateDate);
            if (!string.IsNullOrEmpty(qs["status"]))
                query = query.ByOrderStatus((OrderStatus)(Convert.ToInt32(qs["status"])));
            if (!string.IsNullOrEmpty(qs["isCompleted"]))
                query = query.IsCompleted(Convert.ToBoolean(qs["isCompleted"]));
            if (!string.IsNullOrEmpty(qs["coupon"]))
                query = query.ByCoupon(qs["coupon"]);
            decimal? fromTotal = null, toTotal = null;
            if (!string.IsNullOrEmpty(qs["fromTotal"]))
                fromTotal = Convert.ToDecimal(qs["fromTotal"]);
            if (!string.IsNullOrEmpty(qs["toTotal"]))
                toTotal = Convert.ToDecimal(qs["toTotal"]);
            query = query.ByTotal(fromTotal, toTotal);
            if (!string.IsNullOrEmpty(qs["customField.name"]) && !string.IsNullOrEmpty(qs["customField.value"]))
                query = query.ByCustomField(qs["customField.name"], qs["customField.value"]);

            if (qs["LoadWithCustomer"] == "true")
                query = query.LoadWithCustomer();
            if (qs["LoadWithShoppingCart"] == "true")
                query = query.LoadWithShoppingCart();

            return query;
        }

        /// <summary>
        /// get current logon user's last active order
        /// </summary>
        /// <param name="sessionId">current user's session id</param>
        /// <param name="deleteShoppingCart">whether to delete the shopping cart when order created</param>
        /// <param name="user">current logon user info</param>
        /// <returns>order</returns>
        [HttpPost]
        [Resource("myorder")]
        public Order GetMyOrder(string sessionId, bool deleteShoppingCart, [FromBody]MembershipUser user)
        {
            return Commerce().Orders.GetMyOrder(sessionId, user, deleteShoppingCart);
        }
        protected override ICommerceAccess<Order> GetAccesser()
        {
            return Commerce().Orders;
        }
    }
}
