using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Payments
{
    public interface IPaymentMethodApi
    {
        Query<PaymentMethod> Query();
    }
}
