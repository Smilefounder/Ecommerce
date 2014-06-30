using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.API.Promotions;
using Kooboo.Commerce.API.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.ShoppingCarts
{
    /// <summary>
    /// shopping cart
    /// </summary>
    public class ShoppingCart
    {
        /// <summary>
        /// shopping cart id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// session id
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// shopping cart items
        /// </summary>
        public IList<ShoppingCartItem> Items { get; set; }
        /// <summary>
        /// shipping address
        /// will copy to order's shipping address when create order from shopping cart
        /// </summary>
        public Address ShippingAddress { get; set; }

        public ShippingMethod ShippingMethod { get; set; }

        /// <summary>
        /// billing address
        /// will copy to order's billing address when create order from shopping cart
        public Address BillingAddress { get; set; }
        /// <summary>
        /// customer info
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// use coupon code
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// Applicable promotions for this shopping cart.
        /// </summary>
        public IList<Promotion> AppliedPromotions { get; set; }

        public decimal ShippingCost { get; set; }

        public decimal PaymentMethodCost { get; set; }

        public decimal Tax { get; set; }

        public decimal Subtotal { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal Total { get; set; }

        public ShoppingCart()
        {
            Items = new List<ShoppingCartItem>();
            AppliedPromotions = new List<Promotion>();
        }
    }
}
