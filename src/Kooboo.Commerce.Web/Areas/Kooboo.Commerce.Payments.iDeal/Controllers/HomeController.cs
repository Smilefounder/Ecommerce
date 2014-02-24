using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.iDeal.Controllers
{
    public class HomeController : CommerceControllerBase
    {
        private IPaymentMethodService _paymentMethodService;

        public HomeController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Settings(int methodId, string commerceName)
        {
            var method = _paymentMethodService.GetById(methodId);
            var settings = IDealPaymentGatewayData.Deserialize(method.PaymentGatewayData);
            return View(settings);
        }

        private string GetFullUrl(string url)
        {
            return UrlUtility.Combine(Request.Url.Scheme + "://" + Request.Url.Authority, url);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Settings(int methodId, IDealPaymentGatewayData model, string @return)
        {
            var method = _paymentMethodService.GetById(methodId);
            method.PaymentGatewayData = model.Serialize();

            return AjaxForm().RedirectTo(@return);
        }
    }
}
