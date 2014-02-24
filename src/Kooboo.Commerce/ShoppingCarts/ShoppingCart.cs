using System.Collections.ObjectModel;
using Kooboo.Commerce.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Pricing;

namespace Kooboo.Commerce.ShoppingCarts
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public Guid? GuestId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<ShoppingCartItem> Items { get; set; }

        public virtual Address ShippingAddress { get; set; }

        public virtual Address BillingAddress { get; set; }

        public string CouponCode { get; set; }

        public ShoppingCart()
        {
            Items = new List<ShoppingCartItem>();
        }

        public virtual void AddItem(ProductPrice productPrice, int quantity)
        {
            var item = Items.Where(x => x.ProductPrice.Id == productPrice.Id).FirstOrDefault();

            if (item == null)
            {
                item = new ShoppingCartItem(productPrice, quantity, this);
                Items.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public PricingContext CreatePricingContext()
        {
            var context = new PricingContext
            {
                Customer = Customer,
                CouponCode = CouponCode,
                BillingAddress = BillingAddress,
                ShippingAddress = ShippingAddress
            };

            foreach (var item in Items)
            {
                context.Items.Add(new PricingItem(item.ProductPrice.Product, item.ProductPrice)
                {
                    Quantity = item.Quantity
                });
            }

            return context;
        }
    }
}