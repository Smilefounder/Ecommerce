using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Mapping
{
    public class MappingContext
    {
        public CultureInfo Culture { get; private set; }

        public IncludeCollection Includes { get; private set; }

        public HashSet<ObjectReference> VisitedObjects { get; private set; }

        public MappingContext() : this(null, CultureInfo.CurrentUICulture)
        {
        }

        public MappingContext(IncludeCollection includes)
            : this(includes, CultureInfo.CurrentUICulture)
        {
        }

        public MappingContext(IncludeCollection includes, CultureInfo culture)
        {
            Culture = culture;
            Includes = includes ?? new IncludeCollection();
            VisitedObjects = new HashSet<ObjectReference>();
        }
    }
}
