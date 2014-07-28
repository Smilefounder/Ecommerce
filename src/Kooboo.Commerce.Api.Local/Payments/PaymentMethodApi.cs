using Kooboo.Commerce.Api.Payments;

namespace Kooboo.Commerce.Api.Local.Payments
{
    public class PaymentMethodApi : PaymentMethodQuery, IPaymentMethodApi
    {
        public PaymentMethodApi(LocalApiContext context, Kooboo.Commerce.Payments.IPaymentProcessorProvider processorFactory)
            : base(context, processorFactory)
        {
        }
    }
}
