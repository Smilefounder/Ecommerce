using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Promotions.Policies.Default.Models;
using Kooboo.Commerce.Promotions.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Promotions.Policies.Default.Controllers
{
    public class DiscountPolicyController : CommerceControllerBase
    {
        private IPromotionService _promotionService;

        public DiscountPolicyController(IPromotionService service)
        {
            _promotionService = service;
        }

        public ActionResult Load(int promotionId)
        {
            var promotion = _promotionService.GetById(promotionId);
            var settings = DefaultPromotionPolicyData.Deserialize(promotion.PromotionPolicyData);
            var model = new ConfigModel
            {
                DiscountMode = settings.DiscountMode.ToString(),
                DiscountAppliedTo = settings.DiscountAppliedTo.ToString(),
                DiscountPercent = settings.DiscountPercent,
                DiscountAmount = settings.DiscountAmount
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, HandleAjaxError, Transactional]
        public void Save(int promotionId, ConfigModel model)
        {
            var promotion = _promotionService.GetById(promotionId);
            var policyData = new DefaultPromotionPolicyData
            {
                DiscountMode = (DiscountMode)Enum.Parse(typeof(DiscountMode), model.DiscountMode),
                DiscountAppliedTo = (DiscountAppliedTo)Enum.Parse(typeof(DiscountAppliedTo), model.DiscountAppliedTo),
                DiscountAmount = model.DiscountAmount,
                DiscountPercent = model.DiscountPercent
            };

            promotion.PromotionPolicyData = policyData.Serialize();
        }
    }
}
