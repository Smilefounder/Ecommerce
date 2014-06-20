using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public interface IHasCustomPaymentProcessorConfigEditor
    {
        string GetEditorVirtualPath(PaymentMethod paymentMethod);
    }
}
