using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// Defines methods to resolve parameter value from the context object.
    /// </summary>
    public interface IParameterValueResolver
    {
        /// <summary>
        /// Resolve the parameter value of the specified parameter from the context object.
        /// </summary>
        /// <param name="param">The parameter to resolve value.</param>
        /// <param name="dataContext">The context object.</param>
        /// <returns>The value of the parameter.</returns>
        object ResolveValue(ConditionParameter param, object dataContext);
    }

    /// <summary>
    /// Represents a parameter resolve directly returning the data context as the value of the parameter.
    /// It can be used as the Null-Object pattern to eliminate the boring null checks.
    /// </summary>
    class DumbParameterValueResolver : IParameterValueResolver
    {
        public static readonly DumbParameterValueResolver Instance = new DumbParameterValueResolver();

        public object ResolveValue(ConditionParameter param, object dataContext)
        {
            return dataContext;
        }
    }
}
