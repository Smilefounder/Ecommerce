using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Payments
{
    public class RestPaymentMethodQuery : RestApiBase, IPaymentMethodQuery
    {
        public IPaymentMethodQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        public PaymentMethod[] Pagination(int pageIndex, int pageSize)
        {
            QueryParameters.Add("pageSize", pageSize.ToString());
            QueryParameters.Add("pageIndex", pageIndex.ToString());

            return Get<PaymentMethod[]>(null);
        }

        public PaymentMethod FirstOrDefault()
        {
            return Get<PaymentMethod>("First");
        }

        public PaymentMethod[] ToArray()
        {
            return Get<PaymentMethod[]>(null);
        }

        public int Count()
        {
            return Get<int>("Count");
        }

        protected override string ApiControllerPath
        {
            get { return "PaymentMethod"; }
        }
    }
}
