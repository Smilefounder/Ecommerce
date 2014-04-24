using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Payments
{
    public class RestPaymentQuery : RestApiBase, IPaymentQuery
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

        public IListResource<Payment> Pagination(int pageIndex, int pageSize)
        {
            QueryParameters.Add("pageIndex", pageIndex.ToString());
            QueryParameters.Add("pageSize", pageSize.ToString());
            return Get<ListResource<Payment>>(null);
        }

        public Payment FirstOrDefault()
        {
            return Get<Payment>("First");
        }

        public IListResource<Payment> ToArray()
        {
            return Get<ListResource<Payment>>(null);
        }

        public int Count()
        {
            return Get<int>("Count");
        }

        public ICommerceQuery<Payment> WithoutHalLinks()
        {
            QueryParameters.Add("includeHalLinks", "false");
            return this;
        }

        public ICommerceQuery<Payment> SetHalParameter(string name, object value)
        {
            QueryParameters.Add(string.Format("halParameters.{0}", name), value == null ? "" : value.ToString());
            return this;
        }

        protected override string ApiControllerPath
        {
            get { return "Payment"; }
        }
    }
}
