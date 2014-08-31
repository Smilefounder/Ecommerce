using Kooboo.Commerce.Api.Customers;
using Kooboo.Commerce.Api.Promotions;
using Kooboo.Commerce.Api.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Carts
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public string SessionId { get; set; }

        [OptionalInclude]
        public IList<ShoppingCartItem> Items { get; set; }

        [OptionalInclude]
        public Address ShippingAddress { get; set; }

        [OptionalInclude]
        public Address BillingAddress { get; set; }

        [OptionalInclude]
        public ShippingMethod ShippingMethod { get; set; }

        [OptionalInclude]
        public Customer Customer { get; set; }

        public string CouponCode { get; set; }

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
