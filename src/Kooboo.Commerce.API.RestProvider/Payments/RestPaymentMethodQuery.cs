using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Payments
{
    [Dependency(typeof(IPaymentMethodQuery))]
    public class RestPaymentMethodQuery : RestApiQueryBase<PaymentMethod>, IPaymentMethodQuery
    {
        public IPaymentMethodQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        protected override string ApiControllerPath
        {
            get { return "PaymentMethod"; }
        }
    }
}
