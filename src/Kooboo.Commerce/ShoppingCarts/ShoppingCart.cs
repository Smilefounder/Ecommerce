using Kooboo.Commerce.Customers;
using System.Collections.Generic;
using System.Linq;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Shipping;

namespace Kooboo.Commerce.ShoppingCarts
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public string SessionId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<ShoppingCartItem> Items { get; set; }

        public virtual Address ShippingAddress { get; set; }

        public virtual Address BillingAddress { get; set; }

        public virtual ShippingMethod ShippingMethod { get; set; }

        public string CouponCode { get; set; }

        public ShoppingCart()
        {
            Items = new List<ShoppingCartItem>();
        }

        public static ShoppingCart Create(string sessionId)
        {
            Require.NotNullOrEmpty(sessionId, "sessionId");

            return new ShoppingCart
            {
                SessionId = sessionId
            };
        }

        public static ShoppingCart Create(Customer customer)
        {
            Require.NotNull(customer, "customer");

            return new ShoppingCart
            {
                Customer = customer
            };
        }

        public static ShoppingCart Create(Customer customer, string sessionId)
        {
            Require.NotNull(customer, "customer");

            return new ShoppingCart
            {
                Customer = customer,
                SessionId = sessionId
            };
        }

        public ShoppingCartItem FindItemById(int itemId)
        {
            return Items.FirstOrDefault(i => i.Id == itemId);
        }

        public ShoppingCartItem FindItemByProductPrice(int productPriceId)
        {
            return Items.FirstOrDefault(i => i.ProductPrice.Id == productPriceId);
        }
    }
}