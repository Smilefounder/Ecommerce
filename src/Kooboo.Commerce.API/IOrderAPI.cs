using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Orders;
using Kooboo.CMS.Membership.Models;

namespace Kooboo.Commerce.API
{
    public interface IOrderAPI
    {
        Order GetMyOrder(string sessionId, MembershipUser suer, bool expireShoppingCart = true);

        bool SaveOrder(Order order);
    }
}
