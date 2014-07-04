using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parameters
{
    /// <summary>
    /// 表示静态参数值数据源，可用参数时在创建时就指定并保持不变。
    /// </summary>
    public class StaticRuleParameterValueSource : IRuleParameterValueSource
    {
        public Dictionary<string, string> _values;

        public StaticRuleParameterValueSource(IDictionary<string, string> values)
        {
            Require.NotNull(values, "values");
            _values = new Dictionary<string, string>(values);
        }

        public IDictionary<string, string> GetValues(RuleParameter param)
        {
            return _values;
        }

        public static StaticRuleParameterValueSource FromEnum(Type enumType)
        {
            var values = new Dictionary<string, string>();

            foreach (var name in Enum.GetNames(enumType))
            {
                values.Add(name, name);
            }

            return new StaticRuleParameterValueSource(values);
        }
    }
}
