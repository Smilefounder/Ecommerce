using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// 表示一个可以用在条件表达式里的参数。
    /// </summary>
    public class ConditionParameter
    {
        /// <summary>
        /// 参数名称。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 参数值类型。
        /// </summary>
        public Type ValueType { get; private set; }

        /// <summary>
        /// 用于从上下文中计算参数值的<see cref="Kooboo.Commerce.Rules.IParameterValueResolver"/>。
        /// </summary>
        public IParameterValueResolver ValueResolver { get; private set; }

        /// <summary>
        /// 参数值数据源，若设置，则表示该参数只有固定的几个值可选。
        /// </summary>
        public IParameterValueSource ValueSource { get; private set; }

        /// <summary>
        /// 该参数支持的运算符。
        /// </summary>
        public IList<IComparisonOperator> SupportedOperators { get; private set; }

        public ConditionParameter(string name, Type valueType, IParameterValueResolver valueResolver, IEnumerable<IComparisonOperator> supportedOperators)
            : this(name, valueType, valueResolver, null, supportedOperators)
        {
        }

        public ConditionParameter(string name, Type valueType, IParameterValueResolver valueResolver, IParameterValueSource valueSource, IEnumerable<IComparisonOperator> supportedOperators)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(valueType, "valueType");
            Require.NotNull(valueResolver, "valueResolver");
            Require.NotNull(supportedOperators, "supportedOperators");

            Name = name;
            ValueType = valueType;
            ValueResolver = valueResolver;
            ValueSource = valueSource;
            SupportedOperators = supportedOperators.ToList();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
