using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Payments.Services;

namespace Kooboo.Commerce.API.LocalAPI
{
    [Dependency(typeof(IPaymentMethodAPI), ComponentLifeStyle.Transient, Key = "LocalAPI")]
    public class PaymentMethodAPI : IPaymentMethodAPI
    {
        private IPaymentMethodService _paymentMethodService;

        public PaymentMethodAPI(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        public IEnumerable<PaymentMethod> GetAllPaymentMethods()
        {
            return _paymentMethodService.GetAllPaymentMethods();
        }
    }
}
