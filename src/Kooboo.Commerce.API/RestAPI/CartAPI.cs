using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.ShoppingCarts;
using Kooboo.Commerce.ShoppingCarts.Services;
using Kooboo.Commerce.Customers.Services;
using Kooboo.CMS.Membership.Models;

namespace Kooboo.Commerce.API.RestAPI
{
    [Dependency(typeof(ICartAPI), ComponentLifeStyle.Transient, Key = "RestAPI")]
    public class CartAPI : RestApiBase, ICartAPI
    {
        public bool AddToCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            var form = new Dictionary<string, object>
            {
                { "sessionId", sessionId },
                { "accountId", accountId },
                { "productPriceId", productPriceId },
                { "quantity", quantity },
            };
            return Post<bool>(null, form);
        }

        public bool UpdateCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            var form = new Dictionary<string, object>
            {
                { "sessionId", sessionId },
                { "accountId", accountId },
                { "productPriceId", productPriceId },
                { "quantity", quantity },
            };
            return Put<bool>(null, form);
        }

        public ShoppingCart GetMyCart(string sessionId, string accountId)
        {
            QueryParameters.Add("sessionId", sessionId);
            QueryParameters.Add("accountId", accountId);
            return Get<ShoppingCart>(null);
        }

        public bool ExpireShppingCart(string sessionId, string accountId)
        {
            QueryParameters.Add("sessionId", sessionId);
            QueryParameters.Add("accountId", accountId);
            return Delete<bool>(null);
        }

        protected override string ApiControllerPath
        {
            get { return "Cart"; }
        }
    }
}
