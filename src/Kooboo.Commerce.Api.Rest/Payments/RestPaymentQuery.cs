using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.RestProvider.Payments
{
    [Dependency(typeof(IPaymentQuery))]
    public class RestPaymentQuery : RestApiQueryBase<Payment>, IPaymentQuery
    {
        public IPaymentQuery ById(int id)
        {
            QueryParameters.Add("id", "id");
            return this;
        }

        public IPaymentQuery ByTarget(string targetType, string targetId)
        {
            QueryParameters.Add("targetType", targetType);
            QueryParameters.Add("targetId", targetId);
            return this;
        }

        public IPaymentQuery ByStatus(PaymentStatus status)
        {
            QueryParameters.Add("status", status.ToString());
            return this;
        }

        protected override string ApiControllerPath
        {
            get { return "Payment"; }
        }
    }
}
