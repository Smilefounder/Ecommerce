using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Promotions.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Promotions.Conditions.HasOneProduct.Controllers
{
    public class HomeController : CommerceControllerBase
    {
        [Inject]
        public IPromotionService PromotionService { get; set; }

        public ActionResult Editor(int promotionId, int? requirementId)
        {
            var settings = new HasOneProductConditionData();
            var promotion = PromotionService.GetById(promotionId);

            if (requirementId != null && requirementId.Value > 0)
            {
                var requirement = promotion.FindCondition(requirementId.Value);
                if (!String.IsNullOrEmpty(requirement.ConditionData))
                {
                    settings = HasOneProductConditionData.Deserialize(requirement.ConditionData);
                }
            }

            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
        public void Editor(int promotionId, int? requirementId, HasOneProductConditionData settings)
        {
            var promotion = PromotionService.GetById(promotionId);
            PromotionCondition condition = null;

            if (requirementId != null && requirementId.Value > 0)
            {
                condition = promotion.FindCondition(requirementId.Value);
            }
            else
            {
                condition = new PromotionCondition(promotion, Strings.ConditionName);
                promotion.Conditions.Add(condition);
            }

            condition.ConditionData = settings.Serialize();

            PromotionService.Update(promotion);
        }
    }
}
