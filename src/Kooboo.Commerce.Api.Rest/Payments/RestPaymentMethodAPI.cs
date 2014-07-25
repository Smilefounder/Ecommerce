using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Payments
{
    [Dependency(typeof(IPaymentMethodAPI))]
    public class RestPaymentMethodAPI : RestPaymentMethodQuery, IPaymentMethodAPI
    {
        public IPaymentMethodQuery Query()
        {
            return this;
        }
    }
}
