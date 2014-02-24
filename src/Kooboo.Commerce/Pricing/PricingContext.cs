using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Shipping;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Pricing
{
    public class PricingContext
    {
        public IList<PricingItem> Items { get; set; }

        public Customer Customer { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ShippingMethod ShippingMethod { get; set; }

        public Address ShippingAddress { get; set; }

        public Address BillingAddress { get; set; }

        public string CouponCode { get; set; }

        public decimal Subtotal
        {
            get
            {
                return Items.Sum(x => x.Subtotal);
            }
        }

        public decimal Total { get; set; }

        public decimal Tax { get; set; }

        public decimal ShippingCost { get; set; }

        public decimal PaymentMethodCost { get; set; }

        public IList<Promotion> AppliedPromotions { get; set; }

        public void ApplyTotalDiscount(decimal discount)
        {
            Total = ApplyDiscount(Total, discount);
        }

        public void ApplyTaxDiscount(decimal discount)
        {
            Tax = ApplyDiscount(Tax, discount);
        }

        public void ApplyShippingDiscount(decimal discount)
        {
            ShippingCost = ApplyDiscount(ShippingCost, discount);
        }

        public void ApplyPaymentMethodDiscount(decimal discount)
        {
            PaymentMethodCost = ApplyDiscount(PaymentMethodCost, discount);
        }

        private decimal ApplyDiscount(decimal originalValue, decimal discount)
        {
            var newValue = originalValue - discount;
            if (newValue < 0)
            {
                newValue = 0;
            }

            return newValue;
        }
    }
}
