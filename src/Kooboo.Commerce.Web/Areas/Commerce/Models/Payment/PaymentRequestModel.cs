using Kooboo.Commerce.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Payment
{
    public class PaymentRequestModel
    {
        public int OrderId { get; set; }

        public int PaymentMethodId { get; set; }

        public CreditCardInfo CreditCardInfo { get; set; }

        public string BankId { get; set; }

        public string ReturnUrl { get; set; }
    }
}