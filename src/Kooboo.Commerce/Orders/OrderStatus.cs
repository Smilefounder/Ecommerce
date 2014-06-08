using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders
{
    public enum OrderStatus
    {
        Submitted = 0,
        Paid = 1,
        Processing = 2,
        Cancelled = 3
    }
}
