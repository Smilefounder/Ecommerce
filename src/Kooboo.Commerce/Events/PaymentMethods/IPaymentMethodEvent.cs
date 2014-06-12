using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.PaymentMethods
{
    [Category("Payment Methods", Order = 600)]
    public interface IPaymentMethodEvent : IBusinessEvent
    {
    }
}
