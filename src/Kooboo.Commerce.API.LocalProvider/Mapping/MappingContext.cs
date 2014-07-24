using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Mapping
{
    public class MappingContext
    {
        public HashSet<ObjectProperty> VisitedTargetProperties { get; private set; }

        public MappingContext()
        {
            VisitedTargetProperties = new HashSet<ObjectProperty>();
        }
    }
}
