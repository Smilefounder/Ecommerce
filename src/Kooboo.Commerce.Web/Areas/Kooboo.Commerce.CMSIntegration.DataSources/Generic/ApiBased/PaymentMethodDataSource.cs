using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    [KnownType(typeof(PaymentMethodDataSource))]
    public class PaymentMethodDataSource : ApiBasedDataSource
    {
        public override string Name
        {
            get { return "PaymentMethods"; }
        }

        protected override Type QueryType
        {
            get { return typeof(IPaymentMethodQuery); }
        }

        protected override Type ItemType
        {
            get { return typeof(PaymentMethod); }
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.PaymentMethods;
        }
    }
}