using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public class CreatePaymentResult
    {
        public int PaymentId { get; set; }

        public string RedirectUrl { get; set; }
    }
}
