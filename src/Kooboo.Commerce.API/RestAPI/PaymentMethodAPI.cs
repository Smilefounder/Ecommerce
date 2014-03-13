using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Payments.Services;

namespace Kooboo.Commerce.API.RestAPI
{
    [Dependency(typeof(IPaymentMethodAPI), ComponentLifeStyle.Transient, Key = "RestAPI")]
    public class PaymentMethodAPI : RestApiBase, IPaymentMethodAPI
    {
        public IEnumerable<PaymentMethod> GetAllPaymentMethods()
        {
            return Get<List<PaymentMethod>>(null);
        }

        protected override string ApiControllerPath
        {
            get { return "PaymentMethod"; }
        }
    }
}
