using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public class Payment
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public int PaymentMethodId { get; set; }

        public string PaymentMethodName { get; set; }

        public PaymentStatus Status { get; set; }

        public string ThirdPartyTransactionId { get; set; }

        public int OrderId { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
