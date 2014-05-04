using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    // TODO: Maybe better to remove this and directly add GetAvailableValues to IConditionParameter
    /// <summary>
    /// Represents the data source of a parameter.
    /// If there's a data source can provide values for a parameter,
    /// user will be able to select a value from the data source when configuring the parameter value.
    /// </summary>
    public interface IParameterValueDataSource
    {
        /// <summary>
        /// The unique name of the data source.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The name of the parameter this data source can provide values for.
        /// </summary>
        string TargetParameterName { get; }

        /// <summary>
        /// Retrieve available values for the parameter.
        /// </summary>
        IEnumerable<ParameterValue> GetValues(IConditionParameter parameter);
    }

    public class ParameterValue
    {
        public string Text { get; set; }

        public string Value { get; set; }
    }
}
