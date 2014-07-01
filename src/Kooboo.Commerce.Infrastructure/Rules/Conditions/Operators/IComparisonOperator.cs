using Kooboo.Commerce.Rules.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    /// <summary>
    /// 定义用于条件表达式中的比较运算符接口。
    /// </summary>
    public interface IComparisonOperator
    {
        string Name { get; }

        string Alias { get; }

        /// <summary>
        /// 在参数上应用运算符。
        /// </summary>
        /// <param name="param">参数。</param>
        /// <param name="paramValue">参数值。</param>
        /// <param name="inputValue">用于比较的值。</param>
        /// <returns>比较结果，true表示成功，false表示失败。</returns>
        bool Apply(ConditionParameter param, object paramValue, object inputValue);
    }
}
