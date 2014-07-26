using Kooboo.Commerce.Api.Payments;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Payments.Services;

namespace Kooboo.Commerce.Api.Local.Payments
{
    public class PaymentApi : PaymentQuery, IPaymentApi
    {
        private IPaymentProcessorProvider _processorFactory;

        public PaymentApi(LocalApiContext context, IPaymentProcessorProvider processorFactory)
            : base(context)
        {
            _processorFactory = processorFactory;
        }

        public PaymentResult Pay(PaymentRequest request)
        {
            var paymentMethod = Context.Services.PaymentMethods.GetById(request.PaymentMethodId);
            var payment = new Kooboo.Commerce.Payments.Payment(request.OrderId, request.Amount, paymentMethod, request.Description);

            PaymentService.Create(payment);

            var processor = _processorFactory.FindByName(paymentMethod.ProcessorName);
            object config = null;

            if (processor.ConfigType != null)
            {
                config = paymentMethod.LoadProcessorConfig(processor.ConfigType);
            }

            var processResult = processor.Process(new PaymentProcessingContext(payment, config)
            {
                CurrencyCode = request.CurrencyCode,
                ReturnUrl = request.ReturnUrl,
                Parameters = request.Parameters
            });

            PaymentService.AcceptProcessResult(payment, processResult);

            return new PaymentResult
            {
                Message = processResult.Message,
                PaymentId = payment.Id,
                PaymentStatus = (Kooboo.Commerce.Api.Payments.PaymentStatus)(int)processResult.PaymentStatus,
                RedirectUrl = processResult.RedirectUrl
            };
        }
    }
}
