using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IReferenceResolver
    {
        object Resolve(Type referencingType, object referenceKey);
    }
}
