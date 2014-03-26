using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders
{
    public class PriceCalculationContext
    {
        public IList<PricingItem> Items { get; private set; }

        public Customer Customer { get; set; }

        public string CouponCode { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public decimal Subtotal
        {
            get
            {
                return Items.Sum(x => x.Subtotal);
            }
        }

        public decimal DiscountExItemDiscounts { get; set; }

        public decimal ShippingCost { get; set; }

        public decimal ShippingDiscount { get; set; }

        public decimal PaymentMethodCost { get; set; }

        public decimal PaymentMethodDiscount { get; set; }

        public IList<Promotion> AppliedPromotions { get; private set; }

        public PriceCalculationContext()
        {
            Items = new List<PricingItem>();
            AppliedPromotions = new List<Promotion>();
        }

        public static PriceCalculationContext CreateFrom(ShoppingCart cart)
        {
            var context = new PriceCalculationContext
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

        public static PriceCalculationContext CreateFrom(Order order)
        {
            var context = new PriceCalculationContext
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
