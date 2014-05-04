using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// Represents a parameter to be used in condition expressions.
    /// </summary>
    public interface IConditionParameter
    {
        string Name { get; }

        string DisplayName { get; }

        /// <summary>
        /// The context model type that this parameter is applicable to.
        /// </summary>
        Type ContextModelType { get; }

        /// <summary>
        /// The type of parameter value.
        /// </summary>
        Type ValueType { get; }

        /// <summary>
        /// The comparison operators supported by this parameter.
        /// </summary>
        IEnumerable<IComparisonOperator> SupportedOperators { get; }

        /// <summary>
        /// Retrieve the parameter value from its model.
        /// </summary>
        object GetValue(object model);

        /// <summary>
        /// Parse string value to parameter value with correct type.
        /// </summary>
        object ParseValue(string value);
    }
}
