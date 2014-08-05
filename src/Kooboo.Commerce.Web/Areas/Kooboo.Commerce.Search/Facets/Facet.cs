using System.Collections.Generic;

namespace Kooboo.Commerce.Search.Facets
{
    public class Facet
    {
        public string Name { get; set; }

        public IList<FacetRange> Ranges { get; set; }
    }
}