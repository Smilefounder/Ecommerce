using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Search
{
    public class LocaleFacetFieldNameProvider : IFacetFieldNameProvider
    {
        public string GetMapName(string typeName, string fieldName)
        {
            return string.Format("{0}_{1}", typeName, fieldName);
        }
    }
}
