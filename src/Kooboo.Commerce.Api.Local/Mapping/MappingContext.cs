using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Mapping
{
    public class MappingContext
    {
        public ApiContext ApiContext { get; private set; }

        public IncludeCollection Includes { get; private set; }

        public HashSet<ObjectReference> VisitedObjects { get; private set; }

        public MappingContext(ApiContext apiContext) 
            : this(apiContext, null, CultureInfo.CurrentUICulture)
        {
        }

        public MappingContext(ApiContext apiContext, IncludeCollection includes)
            : this(apiContext, includes, CultureInfo.CurrentUICulture)
        {
        }

        public MappingContext(ApiContext apiContext, IncludeCollection includes, CultureInfo culture)
        {
            ApiContext = apiContext;
            Includes = includes ?? new IncludeCollection();
            VisitedObjects = new HashSet<ObjectReference>();
        }
    }
}
