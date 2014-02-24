using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Shipping.ByWeight.Data;
using Kooboo.Commerce.Shipping.ByWeight.Domain;
using Kooboo.Commerce.Shipping.ByWeight.Models;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Shipping.ByWeight.Controllers
{
    public class HomeController : CommerceControllerBase
    {
        [Inject]
        public AddInDbContext DbContext { get; set; }

        public ActionResult Index(int methodId)
        {
            var rules = DbContext.ByWeightShippingRules
                                 .Where(x => x.ShippingMethodId == methodId)
                                 .OrderBy(x => x.FromWeight)
                                 .ThenBy(x => x.ToWeight)
                                 .ThenBy(x => x.Id)
                                 .ToList()
                                 .Select(x => new ByWeightShippingRuleModel
                                 {
                                     Id = x.Id,
                                     FromWeight = x.FromWeight.ToString("f2"),
                                     ToWeight = x.ToWeight.ToString("f2"),
                                     ShippingPrice = x.ShippingPrice.ToString("f2"),
                                     PriceUnit = x.PriceUnit.ToString()
                                 })
                                 .ToList();

            var model = new ByWeightShippingRulesModel
            {
                ShippingMethodId = methodId,
                Rules = rules
            };

            return View(model);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Index(ByWeightShippingRulesModel model, string @return)
        {
            var currentRules = DbContext.ByWeightShippingRules.Where(x => x.ShippingMethodId == model.ShippingMethodId).ToList();

            // Handle deleted rules
            var deletedRules = currentRules.Where(x => !model.HasRule(x.Id)).ToList();

            foreach (var rule in deletedRules)
            {
                currentRules.Remove(rule);
                DbContext.ByWeightShippingRules.Remove(rule);
            }

            // Handle added and updated rules
            foreach (var ruleModel in model.Rules)
            {
                var currentRule = currentRules.FirstOrDefault(x => x.Id == ruleModel.Id);
                if (currentRule == null)
                {
                    currentRule = new ByWeightShippingRule
                    {
                        ShippingMethodId = model.ShippingMethodId
                    };
                    DbContext.ByWeightShippingRules.Add(currentRule);
                }

                currentRule.FromWeight = decimal.Parse(ruleModel.FromWeight);
                currentRule.ToWeight = decimal.Parse(ruleModel.ToWeight);
                currentRule.ShippingPrice = decimal.Parse(ruleModel.ShippingPrice);
                currentRule.PriceUnit = (ShippingPriceUnit)Enum.Parse(typeof(ShippingPriceUnit), ruleModel.PriceUnit);
            }

            DbContext.SaveChanges();

            return AjaxForm().RedirectTo(@return);
        }
    }
}
