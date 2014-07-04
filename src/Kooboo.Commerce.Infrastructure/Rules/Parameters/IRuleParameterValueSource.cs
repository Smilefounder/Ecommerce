using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    /// <summary>
    /// Defines the methods to retrieve the available values of a rule parameter.
    /// </summary>
    public interface IRuleParameterValueSource
    {
        /// <summary>
        /// Retrieve the available values of the specified rule parameter where the key is the text and the value is the value.
        /// </summary>
        IDictionary<string, string> GetValues(RuleParameter param);
    }
}
