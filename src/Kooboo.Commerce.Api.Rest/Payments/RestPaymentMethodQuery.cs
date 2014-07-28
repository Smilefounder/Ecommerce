using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.RestProvider.Payments
{
    [Dependency(typeof(IPaymentMethodQuery))]
    public class RestPaymentMethodQuery : RestApiQueryBase<PaymentMethod>, IPaymentMethodQuery
    {
        public IPaymentMethodQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        public IPaymentMethodQuery ByUserKey(string userKey)
        {
            QueryParameters.Add("userKey", userKey);
            return this;
        }

        public IPaymentMethodQuery ByName(string name)
        {
            QueryParameters.Add("name", name);
            return this;
        }

        protected override string ApiControllerPath
        {
            get { return "PaymentMethod"; }
        }
    }
}
