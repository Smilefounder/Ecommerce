using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Products
{
    public class PriceRange : IEquatable<PriceRange>
    {
        public decimal Lowest { get; private set; }

        public decimal Highest { get; private set; }

        public PriceRange(decimal lowest, decimal highest)
        {
            Lowest = lowest;
            Highest = highest;
        }

        public bool Equals(PriceRange other)
        {
            return other != null && other.Lowest == Lowest && other.Highest == Highest;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PriceRange);
        }

        public override int GetHashCode()
        {
            return Lowest.GetHashCode() * 397 ^ Highest.GetHashCode();
        }

        public override string ToString()
        {
            return Lowest.ToString("c") + "-" + Highest.ToString("c");
        }
    }
}
