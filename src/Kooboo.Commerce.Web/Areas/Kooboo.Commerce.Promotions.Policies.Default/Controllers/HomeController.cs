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
    public class HomeController : CommerceControllerBase
    {
        [Inject]
        public IPromotionService PromotionService { get; set; }

        public ActionResult Settings(int promotionId)
        {
            var promotion = PromotionService.GetById(promotionId);
            var settings = DefaultPromotionPolicyData.Deserialize(promotion.PromotionPolicyData);
            var model = new DefaultPolicySettingsModel
            {
                DiscountMode = settings.DiscountMode,
                DiscountAppliedTo = settings.DiscountAppliedTo,
                DiscountPercent = settings.DiscountPercent,
                DiscountAmount = settings.DiscountAmount
            };

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
        public ActionResult Settings(int promotionId, DefaultPolicySettingsModel model, string commerceName, string @return)
        {
            var promotion = PromotionService.GetById(promotionId);
            var policyData = new DefaultPromotionPolicyData
            {
                DiscountMode = model.DiscountMode,
                DiscountAppliedTo = model.DiscountAppliedTo,
                DiscountAmount = model.DiscountAmount,
                DiscountPercent = model.DiscountPercent
            };

            promotion.PromotionPolicyData = policyData.Serialize();

            PromotionService.Update(promotion);

            var url = Url.Action("Complete", "Promotion", new
            {
                commerceName = commerceName,
                id = promotionId,
                @return = @return,
                area = "Commerce"
            });

            return AjaxForm().RedirectTo(url);
        }
    }
}
