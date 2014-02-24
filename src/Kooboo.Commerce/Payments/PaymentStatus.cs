using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public enum PaymentStatus
    {
        Pending = 0,
        Success = 1,
        Failed = 2,
        Canceled = 3
    }
}
