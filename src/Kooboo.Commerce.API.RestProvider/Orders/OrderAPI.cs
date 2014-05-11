using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.API.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Orders
{
    /// <summary>
    /// order api
    /// </summary>
    [Dependency(typeof(IOrderAPI), ComponentLifeStyle.Transient)]
    public class OrderAPI : RestApiAccessBase<Order>, IOrderAPI
    {
        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">order id</param>
        /// <returns>order query</returns>
        public IOrderQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        /// <summary>
        /// add customer id filter to query
        /// </summary>
        /// <param name="customerId">customer id</param>
        /// <returns>order query</returns>
        public IOrderQuery ByCustomerId(int customerId)
        {
            QueryParameters.Add("customerId", customerId.ToString());
            return this;
        }

        /// <summary>
        /// add account id filter to query
        /// </summary>
        /// <param name="accountId">account id</param>
        /// <returns>order query</returns>
        public IOrderQuery ByAccountId(string accountId)
        {
            QueryParameters.Add("accountId", accountId);
            return this;
        }

        /// <summary>
        /// add create date filter to query
        /// </summary>
        /// <param name="from">from date filter</param>
        /// <param name="to">to date filter</param>
        /// <returns>order query</returns>
        public IOrderQuery ByCreateDate(DateTime? from, DateTime? to)
        {
            if(from.HasValue)
                QueryParameters.Add("fromCreateDate", from.Value.Ticks.ToString());
            if (to.HasValue)
                QueryParameters.Add("toCreateDate", to.Value.Ticks.ToString());
            return this;
        }

        /// <summary>
        /// add order status filter to query
        /// </summary>
        /// <param name="status">order status</param>
        /// <returns>order query</returns>
        public IOrderQuery ByOrderStatus(OrderStatus status)
        {
            QueryParameters.Add("status", ((int)status).ToString());
            return this;
        }

        /// <summary>
        /// add is completed filter to query
        /// </summary>
        /// <param name="isCompleted">order is completed</param>
        /// <returns>order query</returns>
        public IOrderQuery IsCompleted(bool isCompleted)
        {
            QueryParameters.Add("isCompleted", isCompleted.ToString());
            return this;
        }

        /// <summary>
        /// add coupon filter to query
        /// </summary>
        /// <param name="coupon">order coupon</param>
        /// <returns>order query</returns>
        public IOrderQuery ByCoupon(string coupon)
        {
            QueryParameters.Add("isCompleted", coupon);
            return this;
        }

        /// <summary>
        /// add total filter to query
        /// </summary>
        /// <param name="from">from lower bound of total filter</param>
        /// <param name="to">to upper bound of total filter</param>
        /// <returns>order query</returns>
        public IOrderQuery ByTotal(decimal? from, decimal? to)
        {
            if (from.HasValue)
                QueryParameters.Add("fromTotal", from.Value.ToString());
            if (to.HasValue)
                QueryParameters.Add("toTotal", to.Value.ToString());
            return this;
        }

        /// <summary>
        /// filter by custom field value
        /// </summary>
        /// <param name="customFieldName">custom field name</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>order query</returns>
        public IOrderQuery ByCustomField(string customFieldName, string fieldValue)
        {
            QueryParameters.Add("customField.name", customFieldName);
            QueryParameters.Add("customField.value", fieldValue);
            return this;
        }

        public Order CreateFromShoppingCart(int cartId, MembershipUser user, bool deleteShoppingCart = true)
        {
            QueryParameters.Add("cartId", cartId.ToString());
            QueryParameters.Add("deleteShoppingCart", deleteShoppingCart.ToString());
            return Post<Order>("CreateFromShoppingCart", user);
        }

        /// <summary>
        /// get current logon user's last active order
        /// </summary>
        /// <param name="sessionId">current user's session id</param>
        /// <param name="user">current logon user info</param>
        /// <param name="deleteShoppingCart">whether to delete the shopping cart when order created</param>
        /// <returns>order</returns>
        public Order GetMyOrder(string sessionId, MembershipUser user, bool deleteShoppingCart = true)
        {
            QueryParameters.Add("sessionId", sessionId);
            QueryParameters.Add("deleteShoppingCart", deleteShoppingCart.ToString());
            return Post<Order>("GetMyOrder", user);
        }
        /// <summary>
        /// load order with customer info
        /// </summary>
        /// <returns>order query</returns>
        public IOrderQuery LoadWithCustomer()
        {
            QueryParameters.Add("LoadWithCountry", "true");
            return this;
        }
        /// <summary>
        /// load order with shopping cart info
        /// </summary>
        /// <returns>order query</returns>
        public IOrderQuery LoadWithShoppingCart()
        {
            QueryParameters.Add("LoadWithCountry", "true");
            return this;
        }

        /// <summary>
        /// create order query
        /// </summary>
        /// <returns>order query</returns>
        public IOrderQuery Query()
        {
            return this;
        }

        /// <summary>
        /// create order data access
        /// </summary>
        /// <returns>order data access</returns>
        public IOrderAccess Access()
        {
            return this;
        }

    }
}
