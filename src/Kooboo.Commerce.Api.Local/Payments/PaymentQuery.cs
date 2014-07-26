using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Payments;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Payments.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Payments
{
    [Dependency(typeof(IPaymentQuery))]
    public class PaymentQuery : LocalCommerceQuery<Payment, Kooboo.Commerce.Payments.Payment>, IPaymentQuery
    {
        protected IPaymentService PaymentService { get; private set; }

        public PaymentQuery(IPaymentService paymentService)
        {
            PaymentService = paymentService;
        }

        public IPaymentQuery ById(int id)
        {
            Query = Query.Where(x => x.Id == id);
            return this;
        }

        public IPaymentQuery ByStatus(Kooboo.Commerce.Api.Payments.PaymentStatus status)
        {
            var mappedStatus = (Kooboo.Commerce.Payments.PaymentStatus)(int)status;
            Query = Query.Where(x => x.Status == mappedStatus);
            return this;
        }

        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected override IQueryable<Commerce.Payments.Payment> CreateQuery()
        {
            return PaymentService.Query();
        }

        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected override IQueryable<Commerce.Payments.Payment> OrderByDefault(IQueryable<Commerce.Payments.Payment> query)
        {
            return query.OrderByDescending(x => x.Id);
        }
    }
}
