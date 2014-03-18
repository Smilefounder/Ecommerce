using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Paging;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.EAV.Services;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.EAV;
using Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes;
using Kooboo.Commerce.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{

    public class ProductTypeController : CommerceControllerBase
    {

        private readonly IProductTypeService _productTypeService;
        private readonly ICustomFieldService _customFieldService;

        public ProductTypeController(
                IProductTypeService productTypeService,
                ICustomFieldService customFieldService)
        {
            _productTypeService = productTypeService;
            _customFieldService = customFieldService;
        }

        public ActionResult Index(int? page, int? pageSize)
        {
            var model = _productTypeService.GetAllProductTypes(page, pageSize, o => new ProductTypeRowModel(o));
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
        public ActionResult Enable(ProductTypeRowModel[] model)
        {
            foreach (var item in model)
            {
                var type = _productTypeService.GetById(item.Id);
                _productTypeService.Enable(type);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
        public ActionResult Disable(ProductTypeRowModel[] model)
        {
            foreach (var item in model)
            {
                var type = _productTypeService.GetById(item.Id);
                _productTypeService.Disable(type);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
        public ActionResult Delete(ProductTypeRowModel[] model)
        {
            foreach (var item in model)
            {
                var type = _productTypeService.GetById(item.Id);
                _productTypeService.Delete(type);
            }

            return AjaxForm().ReloadPage();
        }

        public ActionResult Create()
        {
            var model = new ProductTypeEditorModel();
            model.SystemFields = _customFieldService.GetSystemFields().Select(o => new CustomFieldEditorModel(o)).ToList();
            return View(model);
        }

        public ActionResult Edit(int id, string @return)
        {
            var productType = _productTypeService.GetById(id);
            var model = new ProductTypeEditorModel(productType);
            model.SystemFields = _customFieldService.GetSystemFields().Select(o => new CustomFieldEditorModel(o)).ToList();
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
        public ActionResult Save(ProductTypeEditorModel model, string @return)
        {
            var productType = new ProductType();
            model.UpdateTo(productType);
            //
            var updated = false;
            if (model.Id > 0)
            {
                var existType = _productTypeService.GetById(model.Id);
                if (existType != null)
                {
                    _productTypeService.Update(existType, productType);
                    updated = true;
                }
            }
            if (!updated)
            {
                _productTypeService.Create(productType);
            }

            CommerceContext.CurrentInstance.Database.SaveChanges();

            return AjaxForm().RedirectTo(Url.Action("Edit", RouteValues.From(Request.QueryString).Merge("id", productType.Id)));
        }
    }
}
