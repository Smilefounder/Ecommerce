using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.ShoppingCarts.Services;
using Kooboo.CMS.Membership.Models;

namespace Kooboo.Commerce.API.LocalAPI
{
    [Dependency(typeof(IOrderAPI), ComponentLifeStyle.Transient, Key = "LocalAPI")]
    public class OrderAPI : IOrderAPI
    {
        private IOrderService _orderService;
        private IShoppingCartService _shoppingCartService;

        public OrderAPI(IOrderService orderService, IShoppingCartService shoppingCartService)
        {
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
        }

        public Order GetMyOrder(string sessionId, MembershipUser user, bool expireShoppingCart = true)
        {
            var shoppingCart = string.IsNullOrEmpty(sessionId) ? null : _shoppingCartService.GetBySessionId(sessionId);
            if (shoppingCart != null)
            {
                var order = _orderService.GetByShoppingCartId(shoppingCart.Id);
                if (order == null)
                    order = _orderService.CreateOrderFromShoppingCart(shoppingCart, user, expireShoppingCart);
                return order;
            }
            return null;
        }

        public bool SaveOrder(Order order)
        {
            try
            {
                _orderService.Save(order);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
