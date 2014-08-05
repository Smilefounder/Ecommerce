using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Facets
{
    public class FacetResults
    {
        public IDictionary<string, FacetResult> Results { get; set; }

        public FacetResults()
        {
            Results = new Dictionary<string, FacetResult>();
        }
    }

    public class FacetResult
    {
        public IList<FacetValue> Values { get; set; }

        public FacetResult(IEnumerable<FacetValue> values)
        {
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