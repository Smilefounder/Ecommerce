using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.ShoppingCarts
{
    [Dependency(typeof(IShoppingCartAPI), ComponentLifeStyle.Transient)]
    public class ShoppingCartAPI : RestApiQueryBase<ShoppingCart>, IShoppingCartAPI
    {
        public IShoppingCartQuery LoadWithCustomer()
        {
            QueryParameters.Add("LoadWithCustomer", "true");
            return this;
        }

        public IShoppingCartQuery BySessionId(string sessionId)
        {
            QueryParameters.Add("sessionId", sessionId);
            return this;
        }

        public IShoppingCartQuery ByAccountId(string accountId)
        {
            QueryParameters.Add("accountId", accountId);
            return this;
        }

        public bool AddCartItem(int cartId, ShoppingCartItem item)
        {
            if (item != null)
            {
                QueryParameters.Add("cartId", cartId.ToString());
                return Post<bool>("AddCartItem", item);
            }
            return false;
        }

        public bool UpdateCartItem(int cartId, ShoppingCartItem item)
        {
            if (item != null)
            {
                QueryParameters.Add("cartId", cartId.ToString());
                return Post<bool>("UpdateCartItem", item);
            }
            return false;
        }

        public bool RemoveCartItem(int cartId, int cartItemId)
        {
            QueryParameters.Add("cartId", cartId.ToString());
            QueryParameters.Add("cartItemId", cartItemId.ToString());
            return Delete<bool>("RemoveCartItem");
        }

        public bool AddToCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            QueryParameters.Add("sessionId", sessionId);
            QueryParameters.Add("accountId", accountId);
            QueryParameters.Add("productPriceId", productPriceId.ToString());
            QueryParameters.Add("quantity", quantity.ToString());
            return Post<bool>("AddToCart");
        }

        public bool UpdateCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            QueryParameters.Add("sessionId", sessionId);
            QueryParameters.Add("accountId", accountId);
            QueryParameters.Add("productPriceId", productPriceId.ToString());
            QueryParameters.Add("quantity", quantity.ToString());
            return Post<bool>("UpdateCart");
        }

        public bool FillCustomerByAccount(string sessionId, Kooboo.CMS.Membership.Models.MembershipUser user)
        {
            QueryParameters.Add("sessionId", sessionId);
            return Post<bool>("FillCustomerByAccount", user);
        }


        public bool ExpireShppingCart(int shoppingCartId)
        {
            QueryParameters.Add("shoppingCartId", shoppingCartId.ToString());
            return Post<bool>("ExpireShppingCart");
        }

        public IShoppingCartQuery Query()
        {
            return this;
        }

        public IShoppingCartAccess Access()
        {
            return this;
        }
    }
}
