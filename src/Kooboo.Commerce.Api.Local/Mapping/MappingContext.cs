using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Mapping
{
    public class MappingContext
    {
        public IncludeCollection Includes { get; private set; }

        public HashSet<ObjectReference> VisitedObjects { get; private set; }

        public MappingContext() : this(null)
        {
        }

        public MappingContext(IncludeCollection includes)
        {
            Includes = includes ?? new IncludeCollection();
            VisitedObjects = new HashSet<ObjectReference>();
        }
    }
}
