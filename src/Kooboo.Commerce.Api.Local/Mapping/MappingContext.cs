using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Mapping
{
    public class MappingContext
    {
        public LocalApiContext ApiContext { get; private set; }

        public IncludeCollection Includes { get; private set; }

        public HashSet<ObjectReference> VisitedObjects { get; private set; }

        public MappingContext()
            : this(null, null)
        {
        }

        public MappingContext(IncludeCollection includes)
            : this(null, includes)
        {
        }

        public MappingContext(LocalApiContext apiContext) 
            : this(apiContext, null)
        {
        }

        public MappingContext(LocalApiContext apiContext, IncludeCollection includes)
        {
            ApiContext = apiContext;
            Includes = includes ?? new IncludeCollection();
            VisitedObjects = new HashSet<ObjectReference>();
        }
    }
}
