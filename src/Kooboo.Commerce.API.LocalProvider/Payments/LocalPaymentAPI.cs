using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Payments.Services;

using Payment = Kooboo.Commerce.Payments.Payment;
using PaymentDto = Kooboo.Commerce.API.Payments.Payment;
using Kooboo.CMS.Common.Runtime.Dependency;
using Api = Kooboo.Commerce.API;

namespace Kooboo.Commerce.API.LocalProvider.Payments
{
    [Dependency(typeof(IPaymentAPI))]
    public class LocalPaymentAPI : LocalPaymentQuery, IPaymentAPI
    {
        private IPaymentMethodService _paymentMethodService;
        private IPaymentProcessorProvider _processorFactory;

        public LocalPaymentAPI(
            IPaymentMethodService paymentMethodService,
            IPaymentService paymentService,
            IPaymentProcessorProvider processorFactory,
            IMapper<PaymentDto, Payment> mapper)
            : base(paymentService, mapper)
        {
            _processorFactory = processorFactory;
            _paymentMethodService = paymentMethodService;
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
                PaymentStatus = (Api.Payments.PaymentStatus)(int)processResult.PaymentStatus,
                RedirectUrl = processResult.RedirectUrl
            };
        }
    }
}
