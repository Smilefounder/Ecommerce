using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    /// <summary>
    /// 用于将一个对象属性标记条件表达式的参数。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ParamAttribute : Attribute
    {
        /// <summary>
        /// 参数名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数值数据源。
        /// </summary>
        public Type ValueSource { get; set; }
    }
}
