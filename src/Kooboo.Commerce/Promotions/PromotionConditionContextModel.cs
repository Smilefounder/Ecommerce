using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Shipping;
using Kooboo.Commerce.ShoppingCarts;
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
        public ShoppingCartItem Item { get; set; }

        public Customer Customer { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ShippingMethod ShippingMethod { get; set; }

        // TODO: Add ParameterPrefix attribute?
        //       So that we are able to add multiple properties of the same type for the conditions engine
        public Address ShippingAddress { get; set; }

        public Address BillingAddress { get; set; }

        [Parameter]
        public string CouponCode { get; set; }

        public decimal Subtotal { get; set; }
    }
}
