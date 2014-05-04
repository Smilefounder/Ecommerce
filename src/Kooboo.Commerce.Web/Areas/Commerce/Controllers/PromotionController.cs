using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Promotions.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Promotions;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
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

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class PromotionController : CommerceControllerBase
    {
        private IPromotionService _promotionService;
        private IPromotionPolicyFactory _policyFactory;
        private IPromotionPolicyViewsFactory _policyViewsFactory;

        public PromotionController(
            IPromotionService promotionService,
            IPromotionPolicyFactory policyFactory,
            IPromotionPolicyViewsFactory policyViewsFactory)
        {
            _promotionService = promotionService;
            _policyFactory = policyFactory;
            _policyViewsFactory = policyViewsFactory;
        }

        public ActionResult Index(int? page, int? pageSize)
        {
            ViewBag.AllPolicies = _policyFactory.All().ToSelectList().ToList();

            var promotions = _promotionService.Query()
                                             .OrderByDescending(x => x.Id)
                                             .ToPagedList(page, pageSize)
                                             .Transform(x => new PromotionRowModel(x));

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
                var promotionId = each.Id;
                var promotion = _promotionService.GetById(promotionId);
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

            return AjaxForm().RedirectTo(Url.Action("Policy", RouteValues.From(Request.QueryString).Merge("id", promotion.Id)));
        }

        public ActionResult Policy(int id)
        {
            var promotion = _promotionService.GetById(id);
            var views = _policyViewsFactory.FindByPolicyName(promotion.PromotionPolicyName);
            var url = Url.RouteUrl(views.Settings(promotion, ControllerContext), RouteValues.From(Request.QueryString));
            return Redirect(url);
        }

        public ActionResult Complete(int id)
        {
            var promotion = _promotionService.GetById(id);
            return View(promotion);
        }
    }
}
