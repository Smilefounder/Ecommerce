using Kooboo.Commerce.API.HAL;
using Kooboo.Commerce.API.HAL.Persistence;
using Kooboo.Commerce.Web.Areas.Commerce.Models.HAL;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Globalization;
using Kooboo.Commerce.API.HAL.Services;
using Kooboo.Extensions;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions;
using Kooboo.Commerce.Rules.Expressions.Formatting;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class HalController : CommerceControllerBase
    {
        private IResourceDescriptorProvider _resourceDescriptorProvider;
        private IHalRuleService _halRuleService;

        public HalController(IResourceDescriptorProvider resourceDescriptorProvider, IHalRuleService halRuleService)
        {
            _resourceDescriptorProvider = resourceDescriptorProvider;
            _halRuleService = halRuleService;
        }

        public ActionResult ResourceRules(int? page, int? pageSize)
        {
            var rules = _halRuleService.Query()
                                .OrderByDescending(x => x.Id)
                                .ToPagedList(page, pageSize)
                                .Transform(x => new HalRuleRowModel(x));

            return View(rules);
        }

        public ActionResult CreateRule()
        {
            var model = new HalRuleEditorModel();
            ViewBag.CurrentEventType = typeof(HalContext).AssemblyQualifiedNameWithoutVersion();
            ViewBag.Resources = _resourceDescriptorProvider.GetAllDescriptors();
            return View(model);
        }

        public ActionResult EditRule(int id)
        {
            var rule = _halRuleService.GetById(id);
            var model = new HalRuleEditorModel(rule);
            ViewBag.CurrentEventType = typeof(HalContext).AssemblyQualifiedNameWithoutVersion();
            ViewBag.Resources = _resourceDescriptorProvider.GetAllDescriptors();
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult SaveRule(HalRuleEditorModel model, string @return)
        {
            var resources = Request.Form["Resources"].Split(',');
            foreach(var res in resources)
            {
                model.Resources.Add(new HalRuleResourceModel()
                {
                    RuleId = model.Id,
                    ResourceName = res
                });
            }

            HalRule rule = new HalRule();
            model.UpdateTo(rule);
            _halRuleService.Save(rule);

            return AjaxForm().Success().RedirectTo(@return);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult DeleteRule(HalRuleRowModel[] model)
        {
            foreach (var m in model)
            {
                var brand = _halRuleService.GetById(m.Id);
                _halRuleService.Delete(brand);
            }

            return AjaxForm().ReloadPage();
        }

        public ActionResult HighlightConditionsExpression(string expression)
        {
            return JsonNet(new
            {
                ConditionsExpression = expression,
                HighlightedConditionsExpression = new HtmlExpressionFormatter().Format(expression, typeof(HalContext))
            }).UsingClientConvention();
        }
    }
}
