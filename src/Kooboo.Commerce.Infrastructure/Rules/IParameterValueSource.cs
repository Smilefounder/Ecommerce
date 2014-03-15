using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IParameterValueSource
    {
        string Id { get; }

        string ParameterName { get; }

        IEnumerable<ParameterValue> GetValues(IConditionParameter param);
    }

    public class ParameterValue
    {
        public string Text { get; set; }

        public string Value { get; set; }
    }
}
