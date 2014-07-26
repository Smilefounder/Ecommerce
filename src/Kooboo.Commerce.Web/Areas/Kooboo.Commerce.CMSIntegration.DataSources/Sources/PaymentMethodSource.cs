using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public class PaymentMethodSource : ApiCommerceSource
    {
        public PaymentMethodSource()
            : base("PaymentMethods", typeof(IPaymentMethodQuery), typeof(PaymentMethod))
        {
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.PaymentMethods;
        }
    }
}