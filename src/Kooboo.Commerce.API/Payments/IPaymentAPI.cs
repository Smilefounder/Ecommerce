using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public interface IPaymentAPI
    {
        CreatePaymentResult Create(CreatePaymentRequest request);

        IPaymentQuery Query();
    }

    public class CreatePaymentRequest
    {
        public string TargetType { get; set; }

        public string TargetId { get; set; }

        public decimal Amount { get; set; }

        public int PaymentMethodId { get; set; }
    }

    public class CreatePaymentResult
    {
        public int PaymentId { get; set; }

        public string RedirectUrl { get; set; }
    }
}
