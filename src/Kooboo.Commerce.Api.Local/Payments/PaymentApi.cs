using Kooboo.Commerce.Api.Payments;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Payments.Services;

using Payment = Kooboo.Commerce.Payments.Payment;
using PaymentDto = Kooboo.Commerce.Api.Payments.Payment;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Local;

namespace Kooboo.Commerce.Api.Local.Payments
{
    [Dependency(typeof(IPaymentApi))]
    public class PaymentApi : PaymentQuery, IPaymentApi
    {
        private IPaymentMethodService _paymentMethodService;
        private IPaymentProcessorProvider _processorFactory;

        public PaymentApi(LocalApiContext context, IPaymentProcessorProvider processorFactory)
            : base(context.ServiceFactory.Payments)
        {
            _processorFactory = processorFactory;
            _paymentMethodService = context.ServiceFactory.PaymentMethods;
        }

        public PaymentResult Pay(PaymentRequest request)
        {
            var paymentMethod = _paymentMethodService.GetById(request.PaymentMethodId);
            var payment = new Payment(request.OrderId, request.Amount, paymentMethod, request.Description);

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
