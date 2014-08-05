using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Facets
{
    public class FacetRange
    {
        public string Name { get; set; }

        public bool FromInclusive { get; set; }

        public double? FromValue { get; set; }

        public bool ToInclusive { get; set; }

        public double? ToValue { get; set; }

        public FacetRange(string name)
        {
            Name = name;
        }

        // [NULL TO 200]
        // {100 TO 1000]
        public static FacetRange Parse(string name, string rangeValue)
        {
            var parts = rangeValue.Split(new[] { " TO " }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                throw new ArgumentException("Invalid range " + rangeValue + ".");

            var from = parts[0].Trim();
            var to = parts[1].Trim();

            var range = new FacetRange(name);
            range.FromInclusive = IsInclusive(from.First());
            range.ToInclusive = IsInclusive(to.Last());

            from = from.Substring(1);
            to = to.Substring(0, to.Length - 1);

            if (from != "NULL" && from != "*")
            {
                range.FromValue = Convert.ToDouble(from);
            }
            if (to != "NULL" && to != "*")
            {
                range.ToValue = Convert.ToDouble(to);
            }

            return range;
        }

        static bool IsInclusive(char ch)
        {
            switch (ch)
            {
                case '[':
                case ']':
                    return true;
                case '{':
                case '}':
                    return false;
                default:
                    throw new ArgumentException("Could not understand range boundary char " + ch + ".");
            }
        }
    }
}