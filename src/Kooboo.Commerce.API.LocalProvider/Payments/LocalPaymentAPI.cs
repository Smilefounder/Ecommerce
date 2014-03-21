using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Payments.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Web.Url;
using System.Web;

using PaymentMethod = Kooboo.Commerce.Payments.PaymentMethod;
using PaymentMethodReference = Kooboo.Commerce.Payments.PaymentMethodReference;
using Payment = Kooboo.Commerce.Payments.Payment;
using PaymentDto = Kooboo.Commerce.API.Payments.Payment;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.API.LocalProvider.Payments
{
    [Dependency(typeof(IPaymentAPI))]
    public class LocalPaymentAPI : LocalPaymentQuery, IPaymentAccess, IPaymentAPI
    {
        private IPaymentMethodService _paymentMethodService;

        public LocalPaymentAPI(
            IPaymentMethodService paymentMethodService,
            IPaymentService paymentService,
            IMapper<PaymentDto, Payment> mapper)
            : base(paymentService, mapper)
        {
            _paymentMethodService = paymentMethodService;
        }

        public CreatePaymentResult Create(CreatePaymentRequest request)
        {
            var paymentMethod = _paymentMethodService.GetById(request.PaymentMethodId);

            var payment = new Payment
            {
                Description = request.Description,
                PaymentTargetId = request.TargetId,
                PaymentTargetType = request.TargetType,
                Amount = request.Amount,
                PaymentMethod = new PaymentMethodReference(paymentMethod)
            };

            // TODO: How can I call SaveChanges?
            PaymentService.Create(payment);

            return new CreatePaymentResult
            {
                PaymentId = payment.Id,
                RedirectUrl = GetGatewayUrl(payment, request.ReturnUrl)
            };
        }

        private string GetGatewayUrl(Payment payment, string returnUrl)
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
                throw new InvalidOperationException("Requires http context.");

            var url = "/Commerce/Payment/Gateway?paymentId=" + payment.Id
                        + "&commerceName=" + payment.Metadata.CommerceName
                        + "&returnUrl=" + HttpUtility.UrlEncode(returnUrl);
            
            // this is a local implementation, 
            // so the gateway host is always same as the current http context.
            return UrlUtility.Combine(
                httpContext.Request.Url.Scheme + "://" + httpContext.Request.Url.Authority, url);
        }

        public IPaymentQuery Query()
        {
            return this;
        }

        public IPaymentAccess Access()
        {
            return this;
        }
    }
}
