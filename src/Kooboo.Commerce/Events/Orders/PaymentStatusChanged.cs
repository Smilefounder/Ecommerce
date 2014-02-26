using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    /// <summary>
    /// 表示订单支付状态发生变化，不一定非要有真实支付操作发生，也可能是管理员手动修改了支付状态。
    /// 子类事件有可能要求是真实的支付产生的订单支付状态变更。
    /// </summary>
    public class PaymentStatusChanged : IOrderEvent
    {
        public Order Order { get; protected set; }

        public PaymentStatus OldPaymentStatus { get; protected set; }

        public PaymentStatus NewPaymentStatus { get; protected set; }

        public PaymentStatusChanged(Order order, PaymentStatus oldPaymentStatus, PaymentStatus newPaymentStatus)
        {
            Order = order;
            OldPaymentStatus = oldPaymentStatus;
            NewPaymentStatus = newPaymentStatus;
        }
    }
}
