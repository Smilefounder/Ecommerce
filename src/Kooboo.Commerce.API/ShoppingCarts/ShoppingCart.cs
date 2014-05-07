using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.API.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.ShoppingCarts
{
    /// <summary>
    /// shopping cart
    /// </summary>
    public class ShoppingCart : ItemResource
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
        /// customer info
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// shopping cart items
        /// </summary>
        public ShoppingCartItem[] Items { get; set; }
        /// <summary>
        /// shipping address
        /// will copy to order's shipping address when create order from shopping cart
        /// </summary>
        public Address ShippingAddress { get; set; }

        /// <summary>
        /// billing address
        /// will copy to order's billing address when create order from shopping cart
        public Address BillingAddress { get; set; }
        /// <summary>
        /// use coupon code
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// Applicable promotions for this shopping cart.
        /// </summary>
        public Promotion[] AppliedPromotions { get; set; }

        public decimal Subtotal { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal Total { get; set; }
    }
}
