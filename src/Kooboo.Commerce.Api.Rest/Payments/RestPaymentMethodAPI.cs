using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.RestProvider.Payments
{
    [Dependency(typeof(IPaymentMethodApi))]
    public class RestPaymentMethodAPI : RestPaymentMethodQuery, IPaymentMethodApi
    {
        public IPaymentMethodQuery Query()
        {
            return this;
        }
    }
}
