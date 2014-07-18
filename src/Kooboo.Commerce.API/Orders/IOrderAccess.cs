using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.API.Carts;
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
        int CreateFromCart(int cartId, ShoppingContext context);
    }
}
