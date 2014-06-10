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
    public interface IOrderAccess
    {
        Order CreateFromCart(int cartId, MembershipUser user, bool deleteShoppingCart);
    }
}
