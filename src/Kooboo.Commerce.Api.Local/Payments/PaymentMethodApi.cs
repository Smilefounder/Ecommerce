using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Payments;
using Kooboo.Commerce.Payments.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Payments
{
    [Dependency(typeof(IPaymentMethodApi))]
    public class PaymentMethodApi : PaymentMethodQuery, IPaymentMethodApi
    {
        public PaymentMethodApi(
            IPaymentMethodService paymentMethodService, 
            Kooboo.Commerce.Payments.IPaymentProcessorProvider processorFactory)
            : base(paymentMethodService, processorFactory)
        {
        }
    }
}
