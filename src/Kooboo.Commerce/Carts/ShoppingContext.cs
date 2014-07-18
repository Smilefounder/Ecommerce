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
        public string Currency { get; set; }

        [Param]
        public string Culture { get; set; }

        public IDictionary<string, string> Items { get; private set; }

        public ShoppingContext()
        {
            Items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public ShoppingContext(int? customerId, string culture, string currency)
        {
            CustomerId = customerId;
            Culture = culture;
            Currency = currency;
        }

        public ShoppingContext Clone()
        {
            return new ShoppingContext
            {
                CustomerId = CustomerId,
                Currency = Currency,
                Culture = Culture,
                Items = new Dictionary<string, string>(Items)
            };
        }
    }
}
