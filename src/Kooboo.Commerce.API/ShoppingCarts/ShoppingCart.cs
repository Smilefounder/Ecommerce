using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.ShoppingCarts
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public string SessionId { get; set; }

        public Customer Customer { get; set; }
               
        public ShoppingCartItem[] Items { get; set; }
               
        public Address ShippingAddress { get; set; }
               
        public Address BillingAddress { get; set; }

        public string CouponCode { get; set; }
    }
}
