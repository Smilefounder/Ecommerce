using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Facets
{
    public class FacetResult
    {
        public string Name { get; set; }

        public IList<FacetValue> Values { get; set; }

        public FacetResult(string name, IEnumerable<FacetValue> values)
        {
            Name = name;
            Values = values.ToList();
        }
    }

    public class FacetValue
    {
        public string Term { get; set; }

        public int Hits { get; set; }

        public FacetValue(string term, int hits)
        {
            Term = term;
            Hits = hits;
        }

        public override string ToString()
        {
            return Term + ": " + Hits;
        }
    }
}