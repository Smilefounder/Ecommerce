using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    /// <summary>
    /// 表示静态参数值数据源，可用参数时在创建时就指定并保持不变。
    /// </summary>
    public class StaticParameterValueSource : IParameterValueSource
    {
        public List<ParameterValueItem> _values;

        public StaticParameterValueSource(IEnumerable<ParameterValueItem> values)
        {
            Require.NotNull(values, "values");
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
