using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Carts
{
    public class ShoppingContext
    {
        public int? CustomerId { get; set; }

        public string Currency { get; set; }

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
    }
}
