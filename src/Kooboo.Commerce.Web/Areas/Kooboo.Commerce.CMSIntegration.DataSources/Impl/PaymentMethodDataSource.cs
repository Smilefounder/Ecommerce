using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Impl
{
    [DataContract]
    [KnownType(typeof(PaymentMethodDataSource))]
    public class PaymentMethodDataSource : ApiQueryBasedDataSource<PaymentMethod>
    {
        public override string Name
        {
            get { return "PaymentMethods"; }
        }

        protected override Api.Query<PaymentMethod> Query(Api.ICommerceApi api)
        {
            return api.PaymentMethods.Query();
        }
    }
}