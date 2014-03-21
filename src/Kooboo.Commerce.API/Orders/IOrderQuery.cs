using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Orders
{
    public interface IOrderQuery : ICommerceQuery<Order>
    {
        IOrderQuery ById(int id);
        IOrderQuery ByCustomerId(int customerId);

        IOrderQuery ByAccountId(string accountId);

        IOrderQuery ByCreateDate(DateTime? from, DateTime? to);

        IOrderQuery ByOrderStatus(OrderStatus status);

        IOrderQuery IsCompleted(bool isCompleted);

        IOrderQuery ByCoupon(string coupon);

        IOrderQuery ByTotal(decimal? from, decimal? to);

        Order GetMyOrder(string sessionId, MembershipUser user, bool deleteShoppingCart = true);

        IOrderQuery LoadWithCustomer();
        IOrderQuery LoadWithShoppingCart();
    }
}
