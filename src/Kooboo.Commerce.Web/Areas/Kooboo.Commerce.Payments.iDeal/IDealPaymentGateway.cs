using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Web;
using Kooboo.Web.Url;
using Mollie.iDEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.iDeal
{
    [Dependency(typeof(IPaymentGateway), Key = "Kooboo.Commerce.Payments.iDeal.IDealPaymentGateway")]
    public class IDealPaymentGateway : IPaymentGateway
    {
        public string Name
        {
            get
            {
                return Strings.PaymentGatewayName;
            }
        }

        public bool SupportBankSelection
        {
            get
            {
                return true;
            }
        }

        public IEnumerable<BankInfo> GetSupportedBanks(PaymentMethod method)
        {
            var settings = IDealPaymentGatewayData.Deserialize(method.PaymentGatewayData);

            if (String.IsNullOrEmpty(settings.PartnerId))
            {
                return Enumerable.Empty<BankInfo>();
            }

            var banks = new IdealBanks(settings.PartnerId, settings.TestMode);

            if (banks.Error)
                throw new InvalidOperationException("Fail fetching iDeal banks. " + banks.ErrorMessage);

            return banks.Banks.Select(x => new BankInfo(x.Id, x.Name)).ToList();
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request)
        {
            if (request.Amount < (decimal)1.19)
                throw new FormatException("Amount cannot be less than € 1,19");

            var settings = IDealPaymentGatewayData.Deserialize(request.PaymentMethod.PaymentGatewayData);

            var idealFetch = new IdealFetch(
                settings.PartnerId
                , "#" + request.Order.Id
                , UrlUtility.Combine(request.CommerceBaseUrl, Strings.AreaName + "/iDeal/Callback?commerceName=" + request.CommerceName)
                , UrlUtility.Combine(request.CommerceBaseUrl, Strings.AreaName + "/iDeal/Return?commerceName=" + request.CommerceName + "&commerceReturnUrl=" + HttpUtility.UrlEncode(request.ReturnUrl))
                , request.BankId
                , request.Amount
            );

            if (idealFetch.Error)
                throw new PaymentGatewayException(idealFetch.ErrorMessage);

            return ProcessPaymentResult.Pending(idealFetch.TransactionId, idealFetch.Url);
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