using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Carts
{
    public class ShoppingContext
    {
        [Reference(typeof(Customer))]
        public int? CustomerId { get; set; }

        [Param]
        public string Culture { get; set; }

        public IDictionary<string, string> Items { get; private set; }

        public ShoppingContext()
        {
            Items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public ShoppingContext(int? customerId, string culture)
        {
            CustomerId = customerId;
            Culture = culture;
        }

        public ShoppingContext Clone()
        {
            return new ShoppingContext
            {
                CustomerId = CustomerId,
                Culture = Culture,
                Items = new Dictionary<string, string>(Items)
            };
        }
    }
}
