using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Shipping;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing
{
    /// <summary>
    /// Represents the contextual object used in the price calculation pipeline.
    /// </summary>
    public class PricingContext
    {
        public IList<PricingItem> Items { get; set; }

        public Customer Customer { get; set; }

        public string CouponCode { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public PriceWithDiscount PaymentMethodCost { get; private set; }

        public ShippingMethod ShippingMethod { get; set; }

        public PriceWithDiscount ShippingCost { get; private set; }

        public IList<Promotion> AppliedPromotions { get; private set; }

        public PriceWithDiscount Tax { get; private set; }

        public PriceWithDiscount Subtotal { get; private set; }

        public decimal Total
        {
            get
            {
                var total = Subtotal.FinalValue;

                var itemDiscounts = Items.Sum(x => x.Subtotal.Discount);
                if (itemDiscounts > total)
                {
                    itemDiscounts = total;
                }

                total -= itemDiscounts;

                total += PaymentMethodCost.FinalValue;
                total += ShippingCost.FinalValue;
                total += Tax.FinalValue;

                return total;
            }
        }

        public PricingContext()
        {
            Items = new List<PricingItem>();
            ShippingCost = new PriceWithDiscount();
            PaymentMethodCost = new PriceWithDiscount();
            Tax = new PriceWithDiscount();
            Subtotal = new PriceWithDiscount();
            AppliedPromotions = new List<Promotion>();
        }

        public static PricingContext CreateFrom(ShoppingCart cart)
        {
            var context = new PricingContext
            {
                Customer = cart.Customer,
                CouponCode = cart.CouponCode
            };

            foreach (var item in cart.Items)
            {
                context.Items.Add(new PricingItem(item.Id, item.ProductPrice, item.Quantity));
            }

            return context;
        }

        public static PricingContext CreateFrom(Order order)
        {
            var context = new PricingContext
            {
                Customer = order.Customer,
                CouponCode = order.Coupon
            };

            foreach (var item in order.OrderItems)
            {
                context.Items.Add(new PricingItem(item.Id, item.ProductPrice, item.Quantity));
            }

            return context;
        }
    }
}
