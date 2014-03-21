using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.API.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Orders
{
    [Dependency(typeof(IOrderAPI), ComponentLifeStyle.Transient)]
    public class OrderAPI : RestApiAccessBase<Order>, IOrderAPI
    {
        public IOrderQuery LoadWithCustomer()
        {
            QueryParameters.Add("LoadWithCountry", "true");
            return this;
        }
        public IOrderQuery LoadWithShoppingCart()
        {
            QueryParameters.Add("LoadWithCountry", "true");
            return this;
        }

        public IOrderQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        public IOrderQuery ByCustomerId(int customerId)
        {
            QueryParameters.Add("customerId", customerId.ToString());
            return this;
        }

        public IOrderQuery ByAccountId(string accountId)
        {
            QueryParameters.Add("accountId", accountId);
            return this;
        }

        public IOrderQuery ByCreateDate(DateTime? from, DateTime? to)
        {
            if(from.HasValue)
                QueryParameters.Add("fromCreateDate", from.Value.Ticks.ToString());
            if (to.HasValue)
                QueryParameters.Add("toCreateDate", to.Value.Ticks.ToString());
            return this;
        }

        public IOrderQuery ByOrderStatus(OrderStatus status)
        {
            QueryParameters.Add("status", ((int)status).ToString());
            return this;
        }

        public IOrderQuery IsCompleted(bool isCompleted)
        {
            QueryParameters.Add("isCompleted", isCompleted.ToString());
            return this;
        }

        public IOrderQuery ByCoupon(string coupon)
        {
            QueryParameters.Add("isCompleted", coupon);
            return this;
        }

        public IOrderQuery ByTotal(decimal? from, decimal? to)
        {
            if (from.HasValue)
                QueryParameters.Add("fromTotal", from.Value.ToString());
            if (to.HasValue)
                QueryParameters.Add("toTotal", to.Value.ToString());
            return this;
        }


        public Order GetMyOrder(string sessionId, MembershipUser user, bool deleteShoppingCart = true)
        {
            QueryParameters.Add("sessionId", sessionId);
            QueryParameters.Add("deleteShoppingCart", deleteShoppingCart.ToString());
            return Post<Order>("GetMyOrder", user);
        }

        public IOrderQuery Query()
        {
            return this;
        }

        public IOrderAccess Access()
        {
            return this;
        }

    }
}
