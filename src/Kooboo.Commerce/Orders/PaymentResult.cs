using Kooboo.Commerce.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders
{
    public class PaymentResult
    {
        public string Message { get; set; }

        public int PaymentId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public string RedirectUrl { get; set; }
    }
}
