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
        /// 处理支付请求，并返回结果。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ProcessPaymentResult Process(ProcessPaymentRequest request);
        
        /// <summary>
        /// 获取自定义配置数据的编辑器信息。
        /// </summary>
        PaymentProcessorEditor GetEditor(PaymentMethod paymentMethod);
    }
}
