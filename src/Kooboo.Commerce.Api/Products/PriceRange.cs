using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Products
{
    public class PriceRange : IEquatable<PriceRange>
    {
        public decimal From { get; private set; }

        public decimal To { get; private set; }

        public PriceRange(decimal from, decimal to)
        {
            From = from;
            To = to;
        }

        public bool Equals(PriceRange other)
        {
            return other != null && other.From == From && other.To == To;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PriceRange);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return From.GetHashCode() * 397 ^ To.GetHashCode();
            }
        }

        public override string ToString()
        {
            return From.ToString("c") + "-" + To.ToString("c");
        }
    }
}
