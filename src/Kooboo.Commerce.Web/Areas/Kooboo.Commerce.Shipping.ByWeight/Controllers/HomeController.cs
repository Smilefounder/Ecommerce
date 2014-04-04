using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Shipping.ByWeight.Domain;
using Kooboo.Commerce.Shipping.ByWeight.Models;
using Kooboo.Commerce.Shipping.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Controllers;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Shipping.ByWeight.Controllers
{
    public class HomeController : ShippingRateProviderSettingsControllerBase
    {
        private IShippingMethodService _service;

        public HomeController(IShippingMethodService service)
        {
            _service = service;
        }

        public ActionResult Index(int methodId)
        {
            var method = _service.GetById(methodId);
            var rules = new List<ByWeightShippingRuleModel>();

            if (!String.IsNullOrWhiteSpace(method.ShippingRateProviderData))
            {
                rules = JsonConvert.DeserializeObject<List<ByWeightShippingRuleModel>>(method.ShippingRateProviderData);
            }

            var model = new ByWeightShippingRulesModel
            {
                ShippingMethodId = methodId,
                Rules = rules
            };

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Index(ByWeightShippingRulesModel model, string @return)
        {
            var method = _service.GetById(model.ShippingMethodId);
            method.ShippingRateProviderData = JsonConvert.SerializeObject(model.Rules);

            return AjaxForm().RedirectTo(NextStepUrl(method.Id));
        }
    }
}
