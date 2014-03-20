using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Payments.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Url;
using System.Web;

using PaymentMethod = Kooboo.Commerce.Payments.PaymentMethod;
using PaymentMethodReference = Kooboo.Commerce.Payments.PaymentMethodReference;
using Payment = Kooboo.Commerce.Payments.Payment;
using PaymentDto = Kooboo.Commerce.API.Payments.Payment;

namespace Kooboo.Commerce.API.LocalProvider.Payments
{
    public class LocalPaymentAccess : LocalPaymentQuery, IPaymentAccess
    {
        private IPaymentMethodService _paymentMethodService;

        public LocalPaymentAccess(
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
                RedirectUrl = GetGatewayUrl(payment.Id, request.ReturnUrl)
            };
        }

        private string GetGatewayUrl(int paymentId, string returnUrl)
        {
            var baseUrl = Site.Current.GetCommerceUrl();
            return UrlUtility.Combine(baseUrl, 
                "/Commerce/Payment/Gateway?paymentId=" + paymentId
                + "&returnUrl=" + HttpUtility.UrlEncode(returnUrl));
        }
    }
}
