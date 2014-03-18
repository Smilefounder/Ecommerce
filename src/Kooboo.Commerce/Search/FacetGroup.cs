using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Search
{
    public class FacetGroup
    {
        public string FieldName { get; set; }
        public int TotalHits { get; set; }
        public IEnumerable<FacetItem> FacetItems { get; set; }
    }
}
