using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IParameterValueSource
    {
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
