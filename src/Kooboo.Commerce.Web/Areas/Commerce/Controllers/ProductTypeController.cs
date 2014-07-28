using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Paging;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Products;
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
            model.PredefinedFields = _customFieldService.PredefinedFields()
                                                        .ToList()
                                                        .Select(f => new CustomFieldEditorModel(f))
                                                        .ToList();
            return View(model);
        }

        public ActionResult Edit(int id, string @return)
        {
            var productType = _productTypeService.GetById(id);
            var model = new ProductTypeEditorModel(productType);
            model.PredefinedFields = _customFieldService.PredefinedFields()
                                                        .ToList()
                                                        .Select(f => new CustomFieldEditorModel(f))
                                                        .ToList();
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
                if (!field.IsPredefined)
                {
                    var customField = CreateOrUpdateField(field);
                    productType.CustomFields.Add(customField);
                }
                else
                {
                    var customField = _customFieldService.GetById(field.Id);
                    productType.CustomFields.Add(customField);
                }
            }

            foreach (var field in model.VariantFields.OrderBy(f => f.Sequence))
            {
                if (!field.IsPredefined)
                {
                    var variantField = CreateOrUpdateField(field);
                    productType.VariantFields.Add(variantField);
                }
                else
                {
                    var variantField = _customFieldService.GetById(field.Id);
                    productType.VariantFields.Add(variantField);
                }
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

        public ActionResult PredefinedFields()
        {
            var fields = _customFieldService.PredefinedFields()
                                            .OrderBy(f => f.Sequence)
                                            .Select(f => new
                                            {
                                                f.Id,
                                                f.Name,
                                                f.Label,
                                                f.ControlType
                                            });

            return Json(fields, JsonRequestBehavior.AllowGet);
        }
    }
}
