using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public interface IPaymentAccess
    {
        CreatePaymentResult Create(CreatePaymentRequest request);
    }
}
