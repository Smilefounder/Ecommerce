using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Brands;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Web.Framework.Mvc;
using AutoMapper;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class BrandController : CommerceController
    {
        private BrandService _brandService;

        public BrandController(BrandService brandService)
        {
            _brandService = brandService;
        }

        public ActionResult Index(int page = 1, int pageSize = 50)
        {
            var brands = _brandService.Query()
                                      .OrderByDescending(x => x.Id)
                                      .Paginate(page - 1, pageSize)
                                      .Transform(x => new BrandRowModel(x))
                                      .ToPagedList();

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
            var model = Mapper.Map<Brand, BrandEditorModel>(brand);
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Save(BrandEditorModel model, string @return)
        {
            Brand brand = null;

            if (model.Id > 0)
            {
                brand = _brandService.GetById(model.Id);
            }
            else
            {
                brand = new Brand();
            }

            brand.Name = model.Name;
            brand.Description = model.Description;

            brand.CustomFields.Clear();

            foreach (var field in model.CustomFields)
            {
                brand.CustomFields.Add(new BrandCustomField(field.Name, field.Value));
            }

            if (model.Id > 0)
            {
                _brandService.Update(brand);
            }
            else
            {
                _brandService.Create(brand);
            }

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
