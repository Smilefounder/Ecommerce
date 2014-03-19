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

using PaymentMethod = Kooboo.Commerce.Payments.PaymentMethod;
using PaymentMethodReference = Kooboo.Commerce.Payments.PaymentMethodReference;
using Payment = Kooboo.Commerce.Payments.Payment;

namespace Kooboo.Commerce.API.LocalProvider.Payments
{
    public class LocalPaymentAPI : IPaymentAPI
    {
        private ICommerceDatabase _database;

        public LocalPaymentAPI(ICommerceDatabase database)
        {
            _database = database;
        }

        public CreatePaymentResult Create(CreatePaymentRequest request)
        {
            var paymentMethod = _database.GetRepository<PaymentMethod>()
                                         .Get(request.PaymentMethodId);

            var payment = new Payment
            {
                PaymentTargetId = request.TargetId,
                PaymentTargetType = request.TargetType,
                Amount = request.Amount,
                PaymentMethod = new PaymentMethodReference(paymentMethod)
            };

            payment.Create();

            _database.GetRepository<Payment>().Insert(payment);
            _database.SaveChanges();

            return new CreatePaymentResult
            {
                PaymentId = payment.Id,
                RedirectUrl = GetGatewayUrl(payment.Id)
            };
        }

        private string GetGatewayUrl(int paymentId)
        {
            var baseUrl = Site.Current.GetCommerceUrl();
            return UrlUtility.Combine(baseUrl, "/Commerce/Payment/Gateway?paymentId=" + paymentId);
        }
    }
}
