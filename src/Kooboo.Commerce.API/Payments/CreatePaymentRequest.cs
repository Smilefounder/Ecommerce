using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public class CreatePaymentRequest
    {
        public string Description { get; set; }

        public string TargetType { get; set; }

        public string TargetId { get; set; }

        public decimal Amount { get; set; }

        public int PaymentMethodId { get; set; }

        public string ReturnUrl { get; set; }

        // TODO: Credit card info
    }
}
