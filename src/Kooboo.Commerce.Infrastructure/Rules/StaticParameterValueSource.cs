using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class StaticParameterValueSource : IParameterValueSource
    {
        public List<ParameterValueItem> _values;

        public StaticParameterValueSource(IEnumerable<ParameterValueItem> values)
        {
            _values = values.ToList();
        }

        public IEnumerable<ParameterValueItem> GetValues(ConditionParameter param)
        {
            return _values;
        }

        public static StaticParameterValueSource FromEnum(Type enumType)
        {
            var values = new List<ParameterValueItem>();

            foreach (var name in Enum.GetNames(enumType))
            {
                values.Add(new ParameterValueItem(name, name));
            }

            return new StaticParameterValueSource(values);
        }
    }
}
