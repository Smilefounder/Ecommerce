using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Payments;

namespace Kooboo.Commerce.API
{
    public interface IPaymentMethodAPI
    {
        IEnumerable<PaymentMethod> GetAllPaymentMethods();
    }
}
