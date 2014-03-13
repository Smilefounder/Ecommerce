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

namespace Kooboo.Commerce.API.LocalAPI
{
    [Dependency(typeof(ICartAPI), ComponentLifeStyle.Transient, Key = "LocalAPI")]
    public class CartAPI : ICartAPI
    {
        private IShoppingCartService _shoppingCartService;
        private ICustomerService _customerService;

        public CartAPI(IShoppingCartService shoppingCartService, ICustomerService customerService)
        {
            _shoppingCartService = shoppingCartService;
            _customerService = customerService;
        }

        public bool AddToCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            int? customerId = null;
            if (!string.IsNullOrEmpty(accountId))
            {
                var customer = _customerService.GetByAccountId(accountId, false);
               
                if (customer != null)
                {
                    customerId = customer.Id;
                }
            }
            return _shoppingCartService.AddToCart(sessionId, customerId, productPriceId, quantity);
        }

        public bool UpdateCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            int? customerId = null;
            if (!string.IsNullOrEmpty(accountId))
            {
                var customer = _customerService.GetByAccountId(accountId, false);
                if (customer != null)
                {
                    customerId = customer.Id;
                }
            }
            return _shoppingCartService.UpdateCart(sessionId, customerId, productPriceId, quantity);
        }

        public ShoppingCart GetMyCart(string sessionId, string accountId)
        {
            //if (user != null)
            //{
            //    var customer = _customerService.GetByAccountId(accountId, false);
            //    if (customer != null)
            //    {
            //        return _shoppingCartService.GetByCustomer(customer.Id);
            //    }
            //}
            // always get shopping cart from session id
            if (!string.IsNullOrEmpty(sessionId))
                return _shoppingCartService.GetBySessionId(sessionId);

            return null;
        }

        public bool ExpireShppingCart(string sessionId, string accountId)
        {
            var shoppingCart = GetMyCart(sessionId, accountId);
            if (shoppingCart != null)
            {
                return _shoppingCartService.ExpireShppingCart(shoppingCart);
            }
            return false;
        }
    }
}
