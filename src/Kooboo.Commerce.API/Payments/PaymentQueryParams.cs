using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public class PaymentQueryParams
    {
        public int? Id { get; set; }

        public string TargetType { get; set; }

        public string TargetId { get; set; }

        public PaymentStatus? Status { get; set; }
    }
}
