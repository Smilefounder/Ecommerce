using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.ShoppingCarts
{
    public interface IShoppingCartQuery : ICommerceQuery<ShoppingCart>
    {
        IShoppingCartQuery BySessionId(string sessionId);
        IShoppingCartQuery ByAccountId(string accountId);

        IShoppingCartQuery LoadWithCustomer();

        void AddCartItem(int cartId, ShoppingCartItem item);
        void UpdateCartItem(int cartId, ShoppingCartItem item);
        void RemoveCartItem(int cartId, int cartItemId);

        bool AddToCart(string sessionId, string accountId, int productPriceId, int quantity);
        bool UpdateCart(string sessionId, string accountId, int productPriceId, int quantity);
        bool FillCustomerByAccount(string sessionId, MembershipUser user);

    }
}
