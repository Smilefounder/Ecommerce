using System.Collections.Generic;

namespace Kooboo.Commerce.Search
{
    public class FieldFacet
    {
        public string FieldName { get; set; }

        public IList<FacetItem> Items { get; set; }

        public FieldFacet()
        {
            Items = new List<FacetItem>();
        }
    }
}