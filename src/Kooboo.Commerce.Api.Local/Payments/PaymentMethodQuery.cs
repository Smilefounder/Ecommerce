using System.Linq;
using Kooboo.Commerce.Api.Payments;

namespace Kooboo.Commerce.Api.Local.Payments
{
    public class PaymentMethodQuery : LocalCommerceQuery<PaymentMethod, Kooboo.Commerce.Payments.PaymentMethod>, IPaymentMethodQuery
    {
        private Kooboo.Commerce.Payments.IPaymentProcessorProvider _processorFactory;

        public PaymentMethodQuery(LocalApiContext context, Kooboo.Commerce.Payments.IPaymentProcessorProvider processorFactory)
            : base(context)
        {
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
            return Context.Services.PaymentMethods.Query().Where(x => x.IsEnabled);
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
