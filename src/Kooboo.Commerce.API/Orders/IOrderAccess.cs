using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Orders
{
    /// <summary>
    /// order address
    /// </summary>
    public interface IOrderAccess : ICommerceAccess<Order>
    {
        Order CreateFromShoppingCart(int cartId, MembershipUser user, bool deleteShoppingCart);
    }
}
