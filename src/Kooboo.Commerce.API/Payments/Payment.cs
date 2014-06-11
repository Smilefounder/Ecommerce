using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public class Payment : ItemResource
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethodInfo PaymentMethod { get; set; }

        public PaymentStatus Status { get; set; }

        public string ThirdPartyTransactionId { get; set; }

        public PaymentTarget PaymentTarget { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }

    public static class PaymentTargetTypes
    {
        public static readonly string Order = "Order";
    }
}
