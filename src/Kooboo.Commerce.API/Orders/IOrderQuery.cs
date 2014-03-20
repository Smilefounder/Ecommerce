using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Orders
{
    public interface IOrderQuery : ICommerceQuery<Order>, ICommerceAccess<Order>
    {
        IOrderQuery ById(int id);
        IOrderQuery ByCustomerId(int customerId);

        IOrderQuery ByAccountId(string accountId);

        Order GetMyOrder(string sessionId, MembershipUser user, bool deleteShoppingCart = true);

        IOrderQuery LoadWithCustomer();
        IOrderQuery LoadWithShoppingCart();
    }
}
