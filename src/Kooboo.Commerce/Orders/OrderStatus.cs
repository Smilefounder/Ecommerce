using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders
{
    public enum OrderStatus
    {
        Created = 0,
        Paid = 1,
        InProgress = 2,
        Cancelled = 3,
        Completed = 4
    }
}
