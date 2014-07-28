using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Products
{
    public class PriceRange
    {
        public decimal From { get; set; }

        public decimal To { get; set; }

        public PriceRange() { }

        public PriceRange(decimal from, decimal to)
        {
            From = from;
            To = to;
        }
    }
}
