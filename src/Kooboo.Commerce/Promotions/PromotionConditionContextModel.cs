using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Countries;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Shipping;
using Kooboo.Commerce.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    /// <summary>
    /// The contextual model used when evaluating promotion conditions expression.
    /// </summary>
    public class PromotionConditionContextModel
    {
        [Reference(Prefix = "")]
        public PriceCalculationItem Item { get; set; }

        [Reference]
        public Customer Customer { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ShippingMethod ShippingMethod { get; set; }

        [Reference]
        public Address ShippingAddress { get; set; }

        [Reference]
        public Address BillingAddress { get; set; }

        public string CouponCode { get; set; }

        public decimal Subtotal { get; set; }
    }
}
