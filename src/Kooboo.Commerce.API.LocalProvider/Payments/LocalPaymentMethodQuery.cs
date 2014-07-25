using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.Payments.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Payments
{
    [Dependency(typeof(IPaymentMethodQuery))]
    public class LocalPaymentMethodQuery : LocalCommerceQuery<PaymentMethod, Kooboo.Commerce.Payments.PaymentMethod>, IPaymentMethodQuery
    {
        private IPaymentMethodService _paymentMethodService;
        private Kooboo.Commerce.Payments.IPaymentProcessorProvider _processorFactory;

        public LocalPaymentMethodQuery(
            IPaymentMethodService paymentMethodService, 
            Kooboo.Commerce.Payments.IPaymentProcessorProvider processorFactory)
        {
            _paymentMethodService = paymentMethodService;
            _processorFactory = processorFactory;
        }

        public IPaymentMethodQuery ById(int id)
        {
            Query = Query.Where(x => x.Id == id);
            return this;
        }

        public IPaymentMethodQuery ByUserKey(string userKey)
        {
            Query = Query.Where(x => x.UserKey == userKey);
            return this;
        }

        public IPaymentMethodQuery ByName(string name)
        {
            Query = Query.Where(x => x.Name == name);
            return this;
        }

        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected override IQueryable<Commerce.Payments.PaymentMethod> CreateQuery()
        {
            return _paymentMethodService.Query().Where(x => x.IsEnabled);
        }

        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected override IQueryable<Commerce.Payments.PaymentMethod> OrderByDefault(IQueryable<Commerce.Payments.PaymentMethod> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
}
