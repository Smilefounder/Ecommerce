using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.ShoppingCarts
{
    public interface IShoppingCartAPI : IShoppingCartQuery, IShoppingCartAccess
    {
        IShoppingCartQuery Query();
        IShoppingCartAccess Access();
    }
}
