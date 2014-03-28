using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.Payments.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Payments
{
    [Dependency(typeof(IPaymentMethodAPI))]
    public class LocalPaymentMethodAPI : LocalPaymentMethodQuery, IPaymentMethodAPI
    {
        public LocalPaymentMethodAPI(
            IPaymentMethodService paymentMethodService, 
            Kooboo.Commerce.Payments.IPaymentProcessorFactory processorFactory,
            IMapper<PaymentMethod, Kooboo.Commerce.Payments.PaymentMethod> mapper)
            : base(paymentMethodService, processorFactory, mapper)
        {
        }

        public IPaymentMethodQuery Query()
        {
            return this;
        }
    }
}
