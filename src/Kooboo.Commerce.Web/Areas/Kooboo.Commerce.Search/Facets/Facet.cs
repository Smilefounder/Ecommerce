using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Kooboo.Commerce.Search.Facets
{
    [DataContract]
    public class Facet
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IList<FacetRange> Ranges { get; set; }
    }
}