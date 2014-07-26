using Kooboo.Commerce.Api.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Orders
{
    /// <summary>
    /// order api
    /// </summary>
    public interface IOrderApi : IOrderQuery
    {
        int CreateFromCart(int cartId, ShoppingContext context);
    }
}
