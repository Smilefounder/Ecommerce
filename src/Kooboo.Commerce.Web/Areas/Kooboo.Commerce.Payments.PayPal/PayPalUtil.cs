using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.PayPal
{
    public static class PayPalUtil
    {
        public static readonly string GoLiveRedirectUrlFormat = "https://www.paypal.com/webscr?cmd=_ap-payment&paykey={0}";

        public static readonly string SandboxRedirectUrlFormat = "https://www.sandbox.paypal.com/webscr?cmd=_ap-payment&paykey={0}";

        public static string GetRedirectUrl(string payKey, bool sandbox)
        {
            if (sandbox)
            {
                return String.Format(SandboxRedirectUrlFormat, payKey);
            }

            return String.Format(GoLiveRedirectUrlFormat, payKey);
        }

        public static Dictionary<string, string> GetPayPalConfig(PayPalPaymentGatewayData data)
        {
            var config = new Dictionary<string, string>();
            config.Add("mode", data.SandboxMode ? "sandbox" : "live");

            if (data.SandboxMode)
            {
                config.Add("sandboxEmailAddress", "pp.devtools@gmail.com");
            }

            config.Add("account1.apiUsername", data.ApiUserName);
            config.Add("account1.apiPassword", data.ApiPassword);
            config.Add("account1.apiSignature", data.ApiSignature);
            config.Add("account1.applicationId", data.ApplicationId);

            return config;
        }

        public static PaymentStatus GetPaymentStatus(string paymentStatus)
        {
            var result = PaymentStatus.Pending;

            switch (paymentStatus.ToLowerInvariant())
            {
                case "processed":
                case "completed":
                case "canceled_reversal":
                    result = PaymentStatus.Success;
                    break;
                case "denied":
                case "expired":
                case "failed":
                case "voided":
                    result = PaymentStatus.Failed;
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}