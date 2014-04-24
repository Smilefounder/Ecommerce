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

        public IListResource<PaymentMethod> Pagination(int pageIndex, int pageSize)
        {
            QueryParameters.Add("pageSize", pageSize.ToString());
            QueryParameters.Add("pageIndex", pageIndex.ToString());

            return Get<ListResource<PaymentMethod>>(null);
        }

        public PaymentMethod FirstOrDefault()
        {
            return Get<PaymentMethod>("First");
        }

        public IListResource<PaymentMethod> ToArray()
        {
            return Get<ListResource<PaymentMethod>>(null);
        }

        public int Count()
        {
            return Get<int>("Count");
        }

        public ICommerceQuery<PaymentMethod> WithoutHalLinks()
        {
            QueryParameters.Add("includeHalLinks", "false");
            return this;
        }

        public ICommerceQuery<PaymentMethod> SetHalParameter(string name, object value)
        {
            QueryParameters.Add(string.Format("halParameters.{0}", name), value == null ? "" : value.ToString());
            return this;
        }

        protected override string ApiControllerPath
        {
            get { return "PaymentMethod"; }
        }
    }
}
