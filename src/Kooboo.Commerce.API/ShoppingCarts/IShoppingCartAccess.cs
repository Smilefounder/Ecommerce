using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.ShoppingCarts
{
    public interface IShoppingCartAccess
    {
        bool AddCartItem(int cartId, ShoppingCartItem item);
        bool UpdateCartItem(int cartId, ShoppingCartItem item);
        bool RemoveCartItem(int cartId, int cartItemId);

        bool AddToCart(string sessionId, string accountId, int productPriceId, int quantity);
        bool UpdateCart(string sessionId, string accountId, int productPriceId, int quantity);
        bool FillCustomerByAccount(string sessionId, MembershipUser user);
        bool ExpireShppingCart(int shoppingCartId);
    }
}
