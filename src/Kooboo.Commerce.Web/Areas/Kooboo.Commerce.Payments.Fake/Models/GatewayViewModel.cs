using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.Fake.Models
{
    public class GatewayViewModel
    {
        public int OrderId { get; set; }

        public int TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string CommerceReturnUrl { get; set; }
    }
}