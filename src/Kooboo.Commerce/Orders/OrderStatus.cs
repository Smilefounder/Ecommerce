using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders
{
    public enum OrderStatus
    {
        Created = 0,
        PaymentPending = 1,
        Paid = 2,
        Processing = 3,
        Delivered = 4,
        Returned = 5,
        Cancelled = 6
    }
}
