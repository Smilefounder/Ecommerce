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
    public class LocalPaymentAPI : IPaymentAPI
    {
        private IPaymentMethodService _paymentMethodService;
        private IPaymentService _paymentService;
        private IMapper<PaymentDto, Payment> _mapper;

        public LocalPaymentAPI(
            IPaymentMethodService paymentMethodService,
            IPaymentService paymentService,
            IMapper<PaymentDto, Payment> mapper)
        {
            _paymentMethodService = paymentMethodService;
            _paymentService = paymentService;
            _mapper = mapper;
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
            _paymentService.Create(payment);

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

        public IPaymentQuery Query()
        {
            return new LocalPaymentQuery(_paymentService, _mapper);
        }
    }
}
