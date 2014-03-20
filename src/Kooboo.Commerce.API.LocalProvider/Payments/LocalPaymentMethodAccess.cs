using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.Payments.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Payments
{
    [Dependency(typeof(IPaymentMethodAccess))]
    public class LocalPaymentMethodAccess : LocalPaymentMethodQuery, IPaymentMethodAccess
    {
        public LocalPaymentMethodAccess(IPaymentMethodService paymentMethodService, IMapper<PaymentMethod, Kooboo.Commerce.Payments.PaymentMethod> mapper)
            : base(paymentMethodService, mapper)
        {
        }
    }
}
