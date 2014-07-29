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
using AutoMapper;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ProductTypeController : CommerceController
    {
        private readonly IProductTypeService _productTypeService;
        private readonly IPredefinedCustomFieldService _customFieldService;

        public ProductTypeController(IProductTypeService productTypeService, IPredefinedCustomFieldService customFieldService)
        {
            _productTypeService = productTypeService;
            _customFieldService = customFieldService;
        }

        public ActionResult Index(int page = 1, int pageSize = 50)
        {
            var model = _productTypeService.Query()
                                           .Paginate(page - 1, pageSize)
                                           .Transform(type => new ProductTypeModel
                                           {
                                               Id = type.Id,
                                               Name = type.Name,
                                               SkuAlias = type.SkuAlias,
                                               IsEnabled = type.IsEnabled
                                           })
                                           .ToPagedList();
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Enable(ProductTypeModel[] model)
        {
            foreach (var item in model)
            {
                var type = _productTypeService.GetById(item.Id);
                _productTypeService.Enable(type);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Disable(ProductTypeModel[] model)
        {
            foreach (var item in model)
            {
                var type = _productTypeService.GetById(item.Id);
                _productTypeService.Disable(type);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Delete(ProductTypeModel[] model)
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
            var model = Mapper.Map<ProductTypeModel>(new ProductType());
            return View(model);
        }

        public ActionResult Edit(int id, string @return)
        {
            var productType = _productTypeService.GetById(id);
            var model = Mapper.Map<ProductTypeModel>(productType);
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Save(ProductTypeModel model, string @return)
        {
            ProductType productType = null;

            if (model.Id == 0)
            {
                productType = _productTypeService.Create(new CreateProductTypeRequest
                {
                    Name = model.Name,
                    SkuAlias = model.SkuAlias,
                    CustomFields = model.CustomFields,
                    VariantFields = model.VariantFields
                });
            }
            else
            {
                productType = _productTypeService.Update(new UpdateProductTypeRequest(model.Id)
                {
                    Name = model.Name,
                    SkuAlias = model.SkuAlias,
                    CustomFields = model.CustomFields,
                    VariantFields = model.VariantFields
                });
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

        public ActionResult PredefinedFields()
        {
            var fields = _customFieldService.Query()
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
