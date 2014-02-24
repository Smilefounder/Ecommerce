using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.Fake
{
    [Dependency(typeof(IPaymentGateway), Key = "FakePaymentGateway")]
    public class PaymentGateway : IPaymentGateway
    {
        public string Name
        {
            get
            {
                return "Fake";
            }
        }

        public bool SupportBankSelection
        {
            get
            {
                return false;
            }
        }

        public IEnumerable<BankInfo> GetSupportedBanks(PaymentMethod method)
        {
            return Enumerable.Empty<BankInfo>();
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request)
        {
            var gatewayUrl = UrlUtility.Combine(request.CommerceBaseUrl,
                Strings.AreaName + "/Home/Gateway?commerceName=" + request.CommerceName
                + "&orderId=" + request.Order.Id
                + "&amount=" + request.Amount
                + "&currency=" + request.CurrencyCode
                + "&commerceReturnUrl=" + HttpUtility.UrlEncode(request.ReturnUrl));
            return ProcessPaymentResult.Pending(Guid.NewGuid().ToString("N"), gatewayUrl);
        }


        public PaymentGatewayType PaymentGatewayType
        {
            get
            {
                return PaymentGatewayType.RedirectedPayment;
            }
        }
    }
}