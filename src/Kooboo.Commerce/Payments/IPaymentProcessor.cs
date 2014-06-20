using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    /// <summary>
    /// 定义支付处理接口。
    /// </summary>
    public interface IPaymentProcessor
    {
        /// <summary>
        /// 处理器惟一名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 用于存放配置信息的类的类型。
        /// </summary>
        Type ConfigModelType { get; }

        /// <summary>
        /// 处理支付请求，并返回结果。
        /// </summary>
        ProcessPaymentResult Process(PaymentProcessingContext context);
    }
}
