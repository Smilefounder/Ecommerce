using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// Defines the methods to resolve the real referenced object instance of an indirect id reference.
    /// </summary>
    public interface IReferenceResolver
    {
        /// <summary>
        /// Resolve the real referenced object instance by the specified reference key.
        /// </summary>
        /// <param name="referencingType">Type of the real referenced object.</param>
        /// <param name="referenceKey">The reference key to the referenced object.</param>
        /// <returns>The real referenced object instance.</returns>
        object Resolve(Type referencingType, object referenceKey);
    }
}
