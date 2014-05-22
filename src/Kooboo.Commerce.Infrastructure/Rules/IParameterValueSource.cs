using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// Defines the methods to retrieve the available values of a condition parameter.
    /// </summary>
    public interface IParameterValueSource
    {
        /// <summary>
        /// Retrieve the available values of the specified condition parameter.
        /// </summary>
        IEnumerable<ParameterValueItem> GetValues(ConditionParameter param);
    }

    public class ParameterValueItem
    {
        public string Text { get; set; }

        public string Value { get; set; }

        public ParameterValueItem(string text, string value)
        {
            Text = text;
            Value = value;
        }
    }
}
