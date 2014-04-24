using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Brands;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class BrandController : CommerceControllerBase
    {
        private IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public ActionResult Index(int? page, int? pageSize)
        {
            var brands = _brandService.Query()
                                .OrderByDescending(x => x.Id)
                                .ToPagedList(page, pageSize)
                                .Transform(x => new BrandRowModel(x));

            return View(brands);
        }

        public ActionResult Create()
        {
            var model = new BrandEditorModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var brand = _brandService.GetById(id);
            var model = new BrandEditorModel(brand);
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Save(BrandEditorModel model, string @return)
        {
            model.CustomFields = FormHelper.BindToModels<BrandCustomFieldModel>(Request.Form, "CustomFields.");
            Brand brand = new Brand();
            model.UpdateTo(brand);
            _brandService.Save(brand);

            return AjaxForm().RedirectTo(@return);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Delete(BrandRowModel[] model)
        {
            foreach (var m in model)
            {
                var brand = _brandService.GetById(m.Id);
                _brandService.Delete(brand);
            }

            return AjaxForm().ReloadPage();
        }
    }
}
