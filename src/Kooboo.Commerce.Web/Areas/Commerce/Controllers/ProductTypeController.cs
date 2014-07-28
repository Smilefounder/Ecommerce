using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Paging;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.EAV.Services;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.EAV;
using Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes;
using Kooboo.Commerce.Web.Framework.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{

    public class ProductTypeController : CommerceController
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

        public ActionResult Index(int page = 1, int pageSize = 50)
        {
            var model = _productTypeService.Query()
                                           .Paginate(page - 1, pageSize)
                                           .Transform(o => new ProductTypeRowModel(o))
                                           .ToPagedList();
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Enable(ProductTypeRowModel[] model)
        {
            foreach (var item in model)
            {
                var type = _productTypeService.GetById(item.Id);
                _productTypeService.Enable(type);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Disable(ProductTypeRowModel[] model)
        {
            foreach (var item in model)
            {
                var type = _productTypeService.GetById(item.Id);
                _productTypeService.Disable(type);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Delete(ProductTypeRowModel[] model)
        {
            foreach (var item in model)
            {
                var productType = _productTypeService.GetById(item.Id);
                _productTypeService.Delete(productType);
            }

            return AjaxForm().ReloadPage();
        }

        public ActionResult Create()
        {
            var model = new ProductTypeEditorModel();
            model.SystemFields = _customFieldService.PredefinedFields().ToArray();
            return View(model);
        }

        public ActionResult Edit(int id, string @return)
        {
            var productType = _productTypeService.GetById(id);
            var model = new ProductTypeEditorModel(productType);
            model.SystemFields = _customFieldService.PredefinedFields().ToArray();
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Save(ProductTypeEditorModel model, string @return)
        {
            var productType = new ProductType(model.Name, model.SkuAlias)
            {
                Id = model.Id
            };

            // Create or update fields
            foreach (var field in model.CustomFields.OrderBy(f => f.Sequence))
            {
                var customField = CreateOrUpdateField(field);
                productType.CustomFields.Add(customField);
            }

            foreach (var field in model.VariationFields.OrderBy(f => f.Sequence))
            {
                var variantField = CreateOrUpdateField(field);
                productType.VariantFields.Add(variantField);
            }

            if (productType.Id == 0)
            {
                _productTypeService.Create(productType);
            }
            else
            {
                _productTypeService.Update(productType);
            }

            if (model.IsEnabled)
            {
                _productTypeService.Enable(productType);
            }
            else
            {
                _productTypeService.Disable(productType);
            }

            return AjaxForm().RedirectTo(@return);
        }

        private CustomField CreateOrUpdateField(CustomFieldEditorModel model)
        {
            var field = new CustomField
            {
                Id = model.Id
            };

            model.UpdateTo(field);

            if (field.Id == 0)
            {
                _customFieldService.Create(field);
            }
            else
            {
                _customFieldService.Update(field);
            }

            return field;
        }
    }
}
