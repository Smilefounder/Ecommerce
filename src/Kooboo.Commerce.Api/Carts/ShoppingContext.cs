using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Carts
{
    public class ShoppingContext
    {
        public int? CustomerId { get; set; }

        public string Culture { get; set; }

        public IDictionary<string, string> Items { get; private set; }

        public ShoppingContext()
        {
            Items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public ShoppingContext(int? customerId, string culture)
            : this()
        {
            CustomerId = customerId;
            Culture = culture;
        }
    }
}
