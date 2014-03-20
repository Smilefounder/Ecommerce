using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Payments
{
    public class RestPaymentMethodQuery : RestApiBase, IPaymentMethodQuery
    {
        public PaymentMethod[] Pagination(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
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
