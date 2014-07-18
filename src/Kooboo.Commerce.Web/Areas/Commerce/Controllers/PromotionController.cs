using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Promotions.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Promotions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Web.Mvc;
using Kooboo.Globalization;
using Kooboo.Extensions;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Rules.Conditions;
using Kooboo.Commerce.Web.Framework.Mvc.ModelBinding;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class PromotionController : CommerceController
    {
        private IPromotionService _promotionService;
        private IPromotionPolicyProvider _policyProvider;

        public PromotionController(IPromotionService promotionService, IPromotionPolicyProvider policyProvider)
        {
            _promotionService = promotionService;
            _policyProvider = policyProvider;
        }

        public ActionResult Index(int page = 1, int pageSize = 50)
        {
            ViewBag.AllPolicies = _policyProvider.All().ToSelectList().ToList();

            var promotions = _promotionService.Query()
                                             .OrderByDescending(x => x.Id)
                                             .Paginate(page - 1, pageSize)
                                             .Transform(x => new PromotionRowModel(x))
                                             .ToPagedList();

            return View(promotions);
        }

        [HttpPost, Transactional]
        public ActionResult EnablePromotion(int id)
        {
            var promotion = _promotionService.GetById(id);
            _promotionService.Enable(promotion);

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Enable(PromotionRowModel[] model)
        {
            foreach (var each in model)
            {
                var promotionId = each.Id;
                var promotion = _promotionService.GetById(promotionId);
                _promotionService.Enable(promotion);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, Transactional]
        public ActionResult Disable(PromotionRowModel[] model)
        {
            foreach (var each in model)
            {
                var promotionId = each.Id;
                var promotion = _promotionService.GetById(promotionId);
                _promotionService.Disable(promotion);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, Transactional]
        public ActionResult Delete(PromotionRowModel[] model)
        {
            foreach (var each in model)
            {
                var promotion = _promotionService.GetById(each.Id);
                _promotionService.Delete(promotion);
            }

            return AjaxForm().ReloadPage();
        }

        [ChildActionOnly]
        public ActionResult Steps(int step)
        {
            ViewBag.Step = step;
            return PartialView();
        }

        public ActionResult BasicInfo(string policy, int? id)
        {
            var model = new PromotionEditorModel();
            model.PromotionPolicy = policy;

            if (id != null)
            {
                var promotion = _promotionService.GetById(id.Value);
                model.UpdateFrom(promotion);
                model.OtherPromotions = GetOtherPromotions(promotion.Id);

                foreach (var other in promotion.OverlappablePromotions)
                {
                    model.OtherPromotions.First(x => x.Id == other.Id).IsSelected = true;
                }
            }
            else
            {
                model.OtherPromotions = GetOtherPromotions(0);
            }

            return View(model);
        }

        private IList<PromotionRowModel> GetOtherPromotions(int promotionId)
        {
            return _promotionService.Query().Where(x => x.Id != promotionId)
                                            .OrderBy(x => x.Priority)
                                            .ThenBy(x => x.Id)
                                            .ToList(x => new PromotionRowModel(x));
        }

        [HttpPost, Transactional]
        public ActionResult BasicInfo(PromotionEditorModel model)
        {
            var promotion = model.Id > 0 ? _promotionService.GetById(model.Id) : new Promotion();

            model.UpdateSimplePropertiesTo(promotion);

            if (model.Id == 0)
            {
                _promotionService.Create(promotion);
            }

            promotion.OverlappablePromotions.Clear();

            foreach (var other in model.OtherPromotions.Where(x => x.IsSelected))
            {
                promotion.OverlappablePromotions.Add(_promotionService.GetById(other.Id));
            }

            return AjaxForm().RedirectTo(Url.Action("Conditions", RouteValues.From(Request.QueryString).Merge("id", promotion.Id)));
        }

        public ActionResult Conditions(int id)
        {
            var promotion = _promotionService.GetById(id);
            var policy = _policyProvider.FindByName(promotion.PromotionPolicyName);
            var editor = policy as IHasCustomPromotionPolicyConfigEditor;

            string nextUrl = null;

            if (editor != null || policy.ConfigType != null)
            {
                nextUrl = Url.Action("Policy", RouteValues.From(Request.QueryString).Merge("id", promotion.Id));
            }
            else
            {
                nextUrl = Url.Action("Complete", new { id = promotion.Id });
            }

            ViewBag.NextUrl = nextUrl;
            ViewBag.DataContextType = typeof(PromotionConditionContextModel).AssemblyQualifiedNameWithoutVersion();

            return View(promotion);
        }

        [HttpPost, Transactional]
        public void UpdateConditions(int promotionId, IEnumerable<Condition> conditions)
        {
            var promotion = _promotionService.GetById(promotionId);
            promotion.Conditions = conditions;
        }

        public ActionResult Policy(int id)
        {
            var promotion = _promotionService.GetById(id);
            var policy = _policyProvider.FindByName(promotion.PromotionPolicyName);

            var editorModel = new PromotionPolicyConfigEditorModel
            {
                PromotionId = promotion.Id,
                Config = promotion.LoadPolicyConfig(policy.ConfigType) ?? Activator.CreateInstance(policy.ConfigType)
            };

            ViewBag.Policy = policy;
            ViewBag.PolicyConfigEditorModel = editorModel;

            return View(promotion);
        }

        [HttpPost, HandleAjaxError, Transactional]
        public void UpdatePolicyConfig(int promotionId, [ModelBinder(typeof(BindingTypeAwareModelBinder))]object config)
        {
            var promotion = _promotionService.GetById(promotionId);
            promotion.UpdatePolicyConfig(config);
        }

        public ActionResult Complete(int id)
        {
            var promotion = _promotionService.GetById(id);
            return View(promotion);
        }
    }
}
