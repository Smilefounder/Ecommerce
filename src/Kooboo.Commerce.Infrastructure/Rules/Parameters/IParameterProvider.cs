using System;
using System.Collections.Generic;

namespace Kooboo.Commerce.Rules.Parameters
{
    /// <summary>
    /// Defines methods to get the available parameters from a data context type.
    /// </summary>
    public interface IParameterProvider
    {
        /// <summary>
        /// Gets the available parameters of the specified data context type.
        /// </summary>
        IEnumerable<ConditionParameter> GetParameters(Type dataContextType);
    }
}
