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
using Kooboo.Commerce.Web.Mvc.Paging;
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
            Brand brand = null;

            if (model.Id > 0)
            {
                brand = _brandService.GetById(model.Id);
                model.UpdateTo(brand);
            }
            else
            {
                brand = new Brand();
                model.UpdateTo(brand);
                _brandService.Create(brand);
            }

            return AjaxForm().RedirectTo(@return);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Delete(BrandRowModel[] model)
        {
            var ids = model.Select(x => x.Id).ToArray();
            var brands = _brandService.Query()
                .Where(x => ids.Contains(x.Id))
                .ToList();

            foreach (var catalog in brands)
            {
                _brandService.Delete(catalog);
            }

            return AjaxForm().ReloadPage();
        }
    }
}
