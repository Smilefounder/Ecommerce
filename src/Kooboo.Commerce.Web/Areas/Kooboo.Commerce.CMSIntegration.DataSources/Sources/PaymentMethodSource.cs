using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    [Dependency(typeof(ICommerceSource), Key = "PaymentMethods")]
    public class PaymentMethodSource : ApiCommerceSource
    {
        public PaymentMethodSource()
            : base("PaymentMethods", typeof(IPaymentMethodQuery))
        {
        }
    }
}