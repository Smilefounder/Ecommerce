using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Shipping.ByWeight.Models;
using Kooboo.Commerce.Shipping.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Controllers;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Shipping.ByWeight.Controllers
{
    public class ConfigController : CommerceControllerBase
    {
        private IShippingMethodService _service;

        public ConfigController(IShippingMethodService service)
        {
            _service = service;
        }

        public ActionResult Load(int methodId)
        {
            var method = _service.GetById(methodId);
            var config = method.LoadShippingRateProviderConfig<ByWeightShippingRateProviderConfig>() ?? new ByWeightShippingRateProviderConfig();
            var ruleModels = config.Rules.Select(r => new ByWeightShippingRuleModel
            {
                FromWeight = r.FromWeight.ToString(),
                ToWeight = r.ToWeight.ToString(),
                ShippingPrice = r.ShippingPrice.ToString(),
                PriceUnit = r.PriceUnit.ToString()
            })
            .ToList();

            var model = new ByWeightShippingRulesModel
            {
                ShippingMethodId = methodId,
                Rules = ruleModels
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, HandleAjaxError, Transactional]
        public void Save(ByWeightShippingRulesModel model)
        {
            var method = _service.GetById(model.ShippingMethodId);
            var rules = model.Rules.Select(r => new ByWeightShippingRule
            {
                FromWeight = Convert.ToDecimal(r.FromWeight),
                ToWeight = Convert.ToDecimal(r.ToWeight),
                PriceUnit = (ShippingPriceUnit)Enum.Parse(typeof(ShippingPriceUnit), r.PriceUnit),
                ShippingPrice = Convert.ToDecimal(r.ShippingPrice)
            })
            .ToList();

            method.UpdateShippingRateProviderConfig(new ByWeightShippingRateProviderConfig
            {
                Rules = rules
            });
        }
    }
}
