using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public interface IPaymentMethodAPI : IPaymentMethodQuery, IPaymentMethodAccess
    {
        IPaymentMethodQuery Query();
        IPaymentMethodAccess Access();
    }
}
