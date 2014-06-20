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
            var model = _productTypeService.Query()
                                           .ToPagedList(page, pageSize)
                                           .Transform(o => new ProductTypeRowModel(o));
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
                _productTypeService.Delete(item.Id);
            }

            return AjaxForm().ReloadPage();
        }

        public ActionResult Create()
        {
            var model = new ProductTypeEditorModel();
            model.SystemFields = _customFieldService.GetSystemFields().ToArray();
            return View(model);
        }

        public ActionResult Edit(int id, string @return)
        {
            var productType = _productTypeService.GetById(id);
            var model = new ProductTypeEditorModel(productType);
            model.SystemFields = _customFieldService.GetSystemFields().ToArray();
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Save(ProductTypeEditorModel model, string @return)
        {
            // Create new fields

            ProductType productType = null;

            if (model.Id > 0)
            {
                productType = _productTypeService.GetById(model.Id);
            }
            else
            {
                productType = new ProductType(model.Name, model.SkuAlias);
            }

            productType.Name = model.Name;
            productType.SkuAlias = model.SkuAlias;

            // Custom Fields
            foreach (var field in productType.CustomFields.ToList())
            {
                if (!model.CustomFields.Any(f => f.Id == field.CustomFieldId))
                {
                    productType.RemoveCustomField(field.CustomFieldId);
                }
            }

            foreach (var field in model.CustomFields)
            {
                var customField = CreateOrUpdateField(field);
                var current = productType.FindCustomField(customField.Id);
                if (current == null)
                {
                    current = new ProductTypeCustomField(productType, customField);
                    productType.CustomFields.Add(current);
                }
                else
                {
                    // Update sequence
                }
            }

            // Variant Fields
            foreach (var field in productType.VariationFields.ToList())
            {
                if (!model.VariationFields.Any(f => f.Id == field.CustomFieldId))
                {
                    productType.RemoveVariantField(field.CustomFieldId);
                }
            }

            foreach (var field in model.VariationFields)
            {
                var customField = CreateOrUpdateField(field);
                var current = productType.FindVariantField(customField.Id);
                if (current == null)
                {
                    current = new ProductTypeVariantField(productType, customField);
                    productType.VariationFields.Add(current);
                }
                else
                {
                    // Update sequence
                }
            }

            if (productType.Id == 0)
            {
                _productTypeService.Create(productType);
            }

            CommerceContext.CurrentInstance.Database.SaveChanges();

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
            CustomField field = null;

            if (model.Id > 0)
            {
                field = _customFieldService.GetById(model.Id);
            }
            else
            {
                field = new CustomField();
            }

            model.UpdateTo(field);

            if (field.Id == 0)
            {
                _customFieldService.Create(field);
            }

            return field;
        }
    }
}
