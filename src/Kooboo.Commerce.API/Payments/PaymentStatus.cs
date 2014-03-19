using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public enum PaymentStatus
    {
        Pending = 0,

        Success = 1,

        Failed = 2,

        Cancelled = 3,

        /// <summary>
        /// Indicates a reserval credit debit transaction was created.
        /// </summary>
        Reserved = 4
    }
}
