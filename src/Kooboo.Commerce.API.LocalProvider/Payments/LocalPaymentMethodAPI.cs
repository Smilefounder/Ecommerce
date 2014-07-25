﻿using Kooboo.CMS.Common.Runtime.Dependency;
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
            Kooboo.Commerce.Payments.IPaymentProcessorProvider processorFactory)
            : base(paymentMethodService, processorFactory)
        {
        }
    }
}
