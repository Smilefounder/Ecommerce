using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc.Paging;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Products;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.ImageSizes;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Categories.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Categories;
using Kooboo.CMS.Common;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Web.Queries.Products;
using Kooboo.Commerce.Web.Framework.Queries;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Queries;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ProductController : CommerceControllerBase
    {
        private ISettingService _settingService;
        private IProductService _productService;
        private IProductTypeService _productTypeService;
        private IBrandService _brandService;
        private ICategoryService _categoryService;

        public ProductController(
                ISettingService settingService,
                IProductService productService,
                IProductTypeService productTypeService,
                IBrandService brandService,
                ICategoryService categoryService)
        {
            _settingService = settingService;
            _productService = productService;
            _productTypeService = productTypeService;
            _brandService = brandService;
            _categoryService = categoryService;
        }

        public ActionResult Index(string queryName, int? page, int? pageSize)
        {
            var model = new QueryGridModel
            {
                AllQueryInfos = QueryManager.Instance.GetQueryInfos(QueryTypes.Products).ToList()
            };

            if (String.IsNullOrEmpty(queryName))
            {
                model.CurrentQueryInfo = model.AllQueryInfos.FirstOrDefault();
            }
            else
            {
                model.CurrentQueryInfo = QueryManager.Instance.GetQueryInfo(queryName);
            }

            model.CurrentQueryResult = model.CurrentQueryInfo.Query.Execute(CurrentInstance, page ?? 1, pageSize ?? 50, model.CurrentQueryInfo.GetQueryConfig());

            ViewBag.ProductTypes = _productTypeService.Query().ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int productTypeId)
        {
            ViewBag.ProductType = _productTypeService.GetById(productTypeId);

            LoadTabPlugins();

            return View("Edit");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = _productService.GetById(id);
            var productType = _productTypeService.GetById(product.ProductTypeId);

            ViewBag.ProductType = productType;

            LoadTabPlugins();

            return View("Edit");
        }

        [HttpGet]
        public ActionResult Get(int? id = null, int? productTypeId = null)
        {
            Product product = null;
            if (id.HasValue)
            {
                product = _productService.GetById(id.Value);
            }
            if (product == null)
            {
                product = new Product();
                product.ProductTypeId = productTypeId.Value;
                product.Name = "New Product";
            }
            return JsonNet(product);
        }

        [HttpPost]
        public ActionResult Save(Product obj)
        {
            try
            {
                Product product = null;

                if (obj.Id > 0)
                {
                    product = _productService.Update(obj);
                }
                else
                {
                    product = _productService.Create(obj);
                }

                if (obj.IsPublished)
                {
                    _productService.Publish(product);
                }
                else
                {
                    _productService.Unpublish(product);
                }

                return this.JsonNet(new { status = 0, message = "product succssfully saved." });
            }
            catch (Exception ex)
            {
                return this.JsonNet(new { status = 1, message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult ValidateFieldUniqueness(int productId, string fieldName, string fieldValue, string fieldType)
        {
            // Uniqueness is useless for variant fields
            if (fieldValue.Equals("VariantField", StringComparison.OrdinalIgnoreCase))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            var query = _productService.Query()
                                       .Where(p => p.Id != productId)
                                       .Where(p => p.CustomFieldValues.Any(f => f.CustomField.Name == fieldName && f.FieldValue == fieldValue));

            var valid = !query.Any();

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                var product = _productService.GetById(id);
                _productService.Delete(product);
                return this.JsonNet(new JsonResultData()
                {
                    ReloadPage = true
                });
            }
            catch (Exception ex)
            {
                return this.JsonNet(new { status = 1, message = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult Delete(ProductModel[] model)
        {
            try
            {
                foreach (var item in model)
                {
                    var product = _productService.GetById(item.Id);
                    _productService.Delete(product);
                }
                return this.JsonNet(new JsonResultData()
                {
                    ReloadPage = true
                });
            }
            catch (Exception ex)
            {
                return this.JsonNet(new { status = 1, message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetImageTypes()
        {
            var settings = _settingService.Get<ImageSettings>(ImageSettings.Key) ?? new ImageSettings();
            var sizes = settings.Sizes.Where(x => x.IsEnabled).ToList();
            return JsonNet(sizes);
        }

        [HttpGet]
        public ActionResult GetProductType(int id)
        {
            var obj = _productTypeService.GetById(id);
            return JsonNet(obj);
        }

        [HttpGet]
        public ActionResult SearchBrands(string term, int pageSize, int page)
        {
            var query = _brandService.Query();

            if (!String.IsNullOrWhiteSpace(term))
            {
                query = query.Where(p => p.Name.Contains(term));
            }

            var brands = query.OrderBy(p => p.Name)
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .Select(p => new
                              {
                                  p.Id,
                                  p.Name
                              })
                              .ToList();

            return JsonNet(brands);
        }

        [HttpGet]
        public ActionResult GetCategories(int? parentId = null)
        {
            var all = _categoryService.Query().Select(c => new CategoryEntry
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId
            })
            .ToList();

            var tree = CategoryEntry.BuildCategoryTree(parentId, all);

            return JsonNet(tree);
        }
    }
}
