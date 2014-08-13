using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Countries;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Shipping;
using Kooboo.Commerce.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing
{
    /// <summary>
    /// Represents the contextual object used in the price calculation pipeline.
    /// </summary>
    public class PriceCalculationContext
    {
        public List<PriceCalculationItem> Items { get; private set; }

        [Reference]
        public Customer Customer { get; set; }

        [Reference]
        public Address BillingAddress { get; set; }

        [Reference]
        public Address ShippingAddress { get; set; }

        [Param]
        public string Culture { get; set; }

        [Param]
        public string Currency { get; set; }

        [Param]
        public string CouponCode { get; set; }

        [Reference]
        public PaymentMethod PaymentMethod { get; set; }

        public decimal PaymentMethodCost { get; set; }

        [Reference]
        public ShippingMethod ShippingMethod { get; set; }

        public decimal ShippingCost { get; set; }

        public IList<Promotion> AppliedPromotions { get; private set; }

        public decimal Tax { get; set; }

        public decimal Discount { get; set; }

        public decimal Subtotal
        {
            get
            {
                return Items.Sum(i => i.Subtotal);
            }
        }

        public decimal TotalDiscount
        {
            get
            {
                return Items.Sum(i => i.Discount) + Discount;
            }
        }

        public decimal Total
        {
            get
            {
                var total = Subtotal;

                total -= TotalDiscount;

                if (total < 0)
                {
                    total = 0;
                }

                total += PaymentMethodCost;
                total += ShippingCost;
                total += Tax;

                return total;
            }
        }

        public PriceCalculationContext()
        {
            Items = new List<PriceCalculationItem>();
            AppliedPromotions = new List<Promotion>();
        }

        public PriceCalculationItem AddItem(int itemId, ProductVariant productPrice, int quantity)
        {
            var retailPrice = GetFinalUnitPrice(productPrice.ProductId, productPrice.Id, productPrice.Price);
            var item = new PriceCalculationItem(itemId, productPrice.ProductId, productPrice.Id, retailPrice, quantity);
            Items.Add(item);
            return item;
        }

        public decimal GetFinalUnitPrice(int productId, int productPriceId, decimal originalPrice)
        {
            var customerId = Customer == null ? null : (int?)Customer.Id;
            var shoppingContext = new ShoppingContext(customerId, Culture, Currency);
            return PriceCalculationContext.GetFinalPrice(productId, productPriceId, originalPrice, shoppingContext);
        }

        /// <summary>
        /// Gets the final price of the specified product price in the current shopping context.
        /// </summary>
        public static decimal GetFinalPrice(int productId, int variantId, decimal originalPrice, ShoppingContext shoppingContext)
        {
            var @event = new GetPrice(productId, variantId, originalPrice, shoppingContext);
            Event.Raise(@event);
            return @event.FinalPrice;
        }

        public static PriceCalculationContext CreateFrom(ShoppingCart cart)
        {
            var context = new PriceCalculationContext
            {
                Customer = cart.Customer,
                CouponCode = cart.CouponCode,
                BillingAddress = cart.BillingAddress,
                ShippingAddress = cart.ShippingAddress,
                ShippingMethod = cart.ShippingMethod
            };

            foreach (var item in cart.Items)
            {
                context.AddItem(item.Id, item.ProductVariant, item.Quantity);
            }

            return context;
        }
    }
}
