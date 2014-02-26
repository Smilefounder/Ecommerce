using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Orders
{
    public class PaymentFailed : PaymentStatusChanged
    {
        public PaymentFailed(Order order, PaymentStatus oldPaymentStatus)
            : base(order, oldPaymentStatus, PaymentStatus.Failed)
        {
        }
    }
}
