using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.ShoppingCarts;
using Kooboo.CMS.Membership.Models;

namespace Kooboo.Commerce.API
{
    public interface ICartAPI
    {
        bool AddToCart(string sessionId, string accountId, int productPriceId, int quantity);

        bool UpdateCart(string sessionId, string accountId, int productPriceId, int quantity);

        ShoppingCart GetMyCart(string sessionId, string accountId);

        bool ExpireShppingCart(string sessionId, string accountId);
    }
}
