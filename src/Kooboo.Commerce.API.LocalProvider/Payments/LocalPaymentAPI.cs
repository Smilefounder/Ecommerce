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
using System.Configuration;

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
            var url = "/Commerce/Payment/Gateway?paymentId=" + payment.Id
                        + "&commerceName=" + payment.Metadata.CommerceName
                        + "&returnUrl=" + HttpUtility.UrlEncode(returnUrl);

            return UrlUtility.Combine(GetCommerceUrl(), url);
        }

        private string GetCommerceUrl()
        {
            var commerceUrl = ConfigurationManager.AppSettings["CommerceUrl"];
            if (String.IsNullOrEmpty(commerceUrl))
            {
                var httpContext = HttpContext.Current;
                var request = httpContext.Request;
                commerceUrl = request.Url.Scheme + "://" + request.Url.Authority;
            }

            return commerceUrl;
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
