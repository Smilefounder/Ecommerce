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

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ProductController : CommerceControllerBase
    {
        private ICommerceDatabase _db;
        private ISettingService _settingService;
        private IProductService _productService;
        private IProductTypeService _productTypeService;
        private IBrandService _brandService;
        private ICategoryService _categoryService;
        private IExtendedQueryManager _extendedQueryManager;

        public ProductController(
                ICommerceDatabase db,
                ISettingService settingService,
                IProductService productService,
                IProductTypeService productTypeService,
                IBrandService brandService,
                ICategoryService categoryService,
                IExtendedQueryManager extendedQueryManager)
        {
            _db = db;
            _settingService = settingService;
            _productService = productService;
            _productTypeService = productTypeService;
            _brandService = brandService;
            _categoryService = categoryService;
            _extendedQueryManager = extendedQueryManager;
        }

        public ActionResult Index(string search, int? page, int? pageSize)
        {
            var productTypes = _productTypeService.Query().ToList();
            var query = _productService.Query();
            if (!string.IsNullOrEmpty(search))
                query = query.Where(o => o.Name.Contains(search));
            var model = query
                .OrderByDescending(x => x.Id)
                .ToPagedList(page, pageSize);
            ViewBag.ProductTypes = productTypes;
            ViewBag.ExtendedQueries = _extendedQueryManager.GetExtendedQueries<ExtendedQuery.ProductQuery>();

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
                    product = _productService.GetById(obj.Id);
                }
                else
                {
                    product = new Product(obj.Name, _productTypeService.GetById(obj.ProductTypeId));
                }

                // Update basic info
                product.Name = obj.Name;

                if (obj.BrandId != null)
                {
                    product.Brand = _brandService.GetById(obj.BrandId.Value);
                }
                else
                {
                    product.Brand = null;
                }

                product.UpdateCustomFieldValues(obj.CustomFieldValues);
                product.UpdateImages(obj.Images);
                product.UpdateCategories(obj.Categories);

                if (product.Id == 0)
                {
                    _productService.Create(product);
                }
                else
                {
                    _db.SaveChanges();
                    product.NotifyUpdated();
                }

                // Update product prices
                foreach (var price in product.PriceList.ToList())
                {
                    if (!obj.PriceList.Any(p => p.Id == price.Id))
                    {
                        _productService.RemovePrice(product, price.Id);
                    }
                }

                foreach (var priceModel in obj.PriceList)
                {
                    var price = product.FindPrice(priceModel.Id);
                    if (price == null)
                    {
                        price = product.CreatePrice(priceModel.Name, priceModel.Sku);
                        price.UpdateFrom(priceModel);
                        _productService.AddPrice(product, price);
                    }
                    else
                    {
                        price.UpdateFrom(priceModel);
                        _db.SaveChanges();
                        price.NotifyUpdated();
                    }
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
        public ActionResult Delete(ProductRowModel[] model)
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

        [HttpGet]
        public ActionResult ExtendQuery(string name, int? page, int? pageSize)
        {
            var productTypes = _productTypeService.Query().ToList();
            ViewBag.ProductTypes = productTypes;
            ViewBag.ExtendedQueries = _extendedQueryManager.GetExtendedQueries<ExtendedQuery.ProductQuery>();
            IPagedList<Product> model = null;
            var query = _extendedQueryManager.GetExtendedQuery<ExtendedQuery.ProductQuery>(name);
            if (query != null)
            {
                var paras = _extendedQueryManager.GetExtendedQueryParameters<ExtendedQuery.ProductQuery>(name);

                model = query.Query(paras, _db, page ?? 1, pageSize ?? 50)
                    .Transform(o => o.Product);
            }
            else
            {
                var pquery = _productService.Query();
                model = pquery
                    .OrderByDescending(x => x.Id)
                    .ToPagedList(page, pageSize);
            }
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult GetParameters(string name)
        {
            var query = _extendedQueryManager.GetExtendedQuery<ExtendedQuery.ProductQuery>(name);
            var paras = _extendedQueryManager.GetExtendedQueryParameters<ExtendedQuery.ProductQuery>(name);
            return JsonNet(new { Query = query, Parameters = paras });
        }

        [HttpPost]
        public ActionResult SaveParameters(string name, IEnumerable<ExtendedQueryParameter> parameters)
        {
            try
            {
                _extendedQueryManager.SaveExtendedQueryParameters<ExtendedQuery.ProductQuery>(name, parameters);
                return this.JsonNet(new { status = 0, message = "Parameter Saved." });
            }
            catch (Exception ex)
            {
                return this.JsonNet(new { status = 1, message = ex.Message });
            }
        }
    }
}
