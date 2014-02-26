using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Payments;

namespace Kooboo.Commerce.Events.Orders
{
    /// <summary>
    /// Represents an event occurs when a payment is completed.
    /// </summary>
    public class PaymentSucceeded : PaymentStatusChanged
    {
        public PaymentSucceeded(Order order, PaymentStatus oldPaymentStatus)
            : base(order, oldPaymentStatus, PaymentStatus.Success)
        {
        }
    }
}