using Kooboo.Commerce.Api.Payments;

namespace Kooboo.Commerce.Api.Local.Payments
{
    public class PaymentMethodApi : IPaymentMethodApi
    {
        private LocalApiContext _context;

        public PaymentMethodApi(LocalApiContext context)
        {
            _context = context;
        }

        public Query<PaymentMethod> Query()
        {
            return new Query<PaymentMethod>(new PaymentMethodQueryExecutor(_context));
        }
    }
}
