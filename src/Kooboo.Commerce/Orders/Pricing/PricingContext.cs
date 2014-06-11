using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Rules;
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
        private List<PricingItem> _items = new List<PricingItem>();

        public IEnumerable<PricingItem> Items
        {
            get
            {
                return _items;
            }
        }

        [Reference]
        public Customer Customer { get; set; }

        [Param]
        public string Culture { get; set; }

        [Param]
        public string Currency { get; set; }

        [Param]
        public string CouponCode { get; set; }

        [Reference]
        public PaymentMethod PaymentMethod { get; set; }

        public PriceWithDiscount PaymentMethodCost { get; private set; }

        [Reference]
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

                var itemDiscounts = _items.Sum(x => x.Subtotal.Discount);
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
            _items = new List<PricingItem>();
            ShippingCost = new PriceWithDiscount();
            PaymentMethodCost = new PriceWithDiscount();
            Tax = new PriceWithDiscount();
            Subtotal = new PriceWithDiscount();
            AppliedPromotions = new List<Promotion>();
        }

        public PricingItem AddPricingItem(int itemId, ProductPrice productPrice, int quantity)
        {
            var retailPrice = GetFinalRetialPrice(productPrice.ProductId, productPrice.Id, productPrice.RetailPrice);
            var item = new PricingItem(itemId, productPrice.ProductId, productPrice.Id, retailPrice, quantity);
            _items.Add(item);
            return item;
        }

        public decimal GetFinalRetialPrice(int productId, int productPriceId, decimal originalPrice)
        {
            var customerId = Customer == null ? null : (int?)Customer.Id;
            var shoppingContext = new ShoppingContext(customerId, Culture, Currency);
            return PricingContext.GetFinalRetailPrice(productId, productPriceId, originalPrice, shoppingContext);
        }

        /// <summary>
        /// Gets the final price of the specified product price in the current shopping context.
        /// </summary>
        public static decimal GetFinalRetailPrice(int productId, int productPriceId, decimal originalPrice, ShoppingContext shoppingContext)
        {
            var @event = new GetPrice(productId, productPriceId, originalPrice, shoppingContext);
            Event.Raise(@event);
            return @event.FinalPrice;
        }

        #region Scope

        public static PricingContext GetCurrent()
        {
            return Scope.Current<PricingContext>();
        }

        /// <summary>
        /// Begin a scope for PricingContext, so current PricingContext can be accessed via PricingContext.GetCurrent() within the scope in the same thread.
        /// </summary>
        public static Scope<PricingContext> Begin(PricingContext context)
        {
            Require.NotNull(context, "context");
            return Scope.Begin<PricingContext>(context);
        }

        #endregion

        public static PricingContext CreateFrom(ShoppingCart cart)
        {
            var context = new PricingContext
            {
                Customer = cart.Customer,
                CouponCode = cart.CouponCode
            };

            foreach (var item in cart.Items)
            {
                context.AddPricingItem(item.Id, item.ProductPrice, item.Quantity);
            }

            return context;
        }
    }
}
