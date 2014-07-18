using Kooboo.Commerce.API.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Orders
{
    /// <summary>
    /// order api
    /// </summary>
    public interface IOrderAPI : IOrderQuery
    {
        int CreateFromCart(int cartId, ShoppingContext context);
    }
}
