using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Payments
{
    public class RestPaymentQuery : RestApiBase, IPaymentQuery
    {
        public IPaymentQuery By(PaymentQueryParams parameters)
        {
            if (parameters.Id != null)
            {
                ById(parameters.Id.Value);
            }
            if (parameters.Status != null)
            {
                ByStatus(parameters.Status.Value);
            }
            if (!String.IsNullOrEmpty(parameters.TargetType) && !String.IsNullOrEmpty(parameters.TargetId))
            {
                ByTarget(parameters.TargetType, parameters.TargetId);
            }

            return this;
        }

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

        public Payment[] Pagination(int pageIndex, int pageSize)
        {
            QueryParameters.Add("pageIndex", pageIndex.ToString());
            QueryParameters.Add("pageSize", pageSize.ToString());
            return Get<Payment[]>(null);
        }

        public Payment FirstOrDefault()
        {
            return Get<Payment>("First");
        }

        public Payment[] ToArray()
        {
            return Get<Payment[]>(null);
        }

        public int Count()
        {
            return Get<int>("Count");
        }

        protected override string ApiControllerPath
        {
            get { return "Payment"; }
        }
    }
}
