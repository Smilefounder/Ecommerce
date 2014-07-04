using Kooboo.Commerce.Rules.Parameters;
using System;

namespace Kooboo.Commerce.Rules
{
    // TODO: Is there a better name?
    /// <summary>
    /// 用于将一个对象属性标记为对象引用。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ReferenceAttribute : Attribute
    {
        /// <summary>
        /// 参数前缀。
        /// 例如，Order中的BillingAddress和ShippingAddress都引用了OrderAddress，
        /// 此时可分别设置前缀为Billing和Shipping，这样便可以得到 OrderBillingCity 和 OrderShippingCity 这样不同的参数。
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 引用的对象属性，当引用是通过Key间接引用时，需要显式设置引用对象类型。
        /// </summary>
        public Type ReferencingType { get; set; }

        /// <summary>
        /// 引用对象值求解器，仅当引用是通过Key间接引用时有需要。
        /// </summary>
        public Type ReferenceResolver { get; set; }

        /// <summary>
        /// 标记一个对象属性为对象引用。
        /// </summary>
        public ReferenceAttribute() { }

        /// <summary>
        /// 标记一个对象属性为对另一个对象的间接引用。
        /// </summary>
        /// <param name="referencingType">引用的对象的实际类型。</param>
        public ReferenceAttribute(Type referencingType)
        {
            ReferencingType = referencingType;
            ReferenceResolver = typeof(DefaultIndirectReferenceResolver);
        }
    }
}
