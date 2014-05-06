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
using Kooboo.Commerce.Web.Mvc.Controllers;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.ImageSizes;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Categories.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Categories;
using Kooboo.CMS.Common;
using Kooboo.Commerce.ImageSizes.Services;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{

    public class ProductController : CommerceControllerBase
    {

        private readonly ICommerceDatabase _db;
        private readonly IProductService _productService;
        private readonly IImageSizeService _imageSizeService;
        private readonly IProductTypeService _productTypeService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly IExtendedQueryManager _extendedQueryManager;

        public ProductController(ICommerceDatabase db,
                IProductService productService,
                IImageSizeService imageSizeService,
                IProductTypeService productTypeService,
                IBrandService brandService,
                ICategoryService categoryService,
                IExtendedQueryManager extendedQueryManager)
        {
            _db = db;
            _productService = productService;
            _imageSizeService = imageSizeService;
            _productTypeService = productTypeService;
            _brandService = brandService;
            _categoryService = categoryService;
            _extendedQueryManager = extendedQueryManager;
        }

        public ActionResult Index(string search, int? page, int? pageSize)
        {
            var productTypes = _productTypeService.GetAllProductTypes();
            var query = _productService.Query();
            if(!string.IsNullOrEmpty(search))
                query = query.Where(o => o.Name.Contains(search));
            var model = query
                .OrderByDescending(x => x.Id)
                .ToPagedList(page, pageSize);
            ViewBag.ProductTypes = productTypes.ToList();
            ViewBag.ExtendedQueries = _extendedQueryManager.GetExtendedQueries<Product, Product>();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Edit");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
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
                _productService.Save(obj);
                return this.JsonNet(new { status = 0, message = "product succssfully saved." });
            }
            catch (Exception ex)
            {
                return this.JsonNet(new { status = 1, message = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                var product = _productService.GetById(id);
                if (product != null)
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
                    if (product != null)
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
            var sizes = _imageSizeService.Query()
                                         .Where(x => x.IsEnabled)
                                         .ToList();
            return JsonNet(sizes);
        }

        [HttpGet]
        public ActionResult GetProductType(int id)
        {
            var obj = _productTypeService.GetById(id);
            return JsonNet(obj);
        }

        [HttpGet]
        public ActionResult GetBrands()
        {

            var objs = _brandService.Query();
            return JsonNet(objs);
        }

        [HttpGet]
        public ActionResult GetCategories(int? parentId = null)
        {

            var query = _categoryService.Query();
            if(parentId.HasValue)
                query = query.Where(o => o.Parent.Id == parentId.Value);
            else
                query = query.Where(o => o.Parent == null);
            var objs = query.ToArray();
            return JsonNet(objs);
        }

        [HttpGet]
        public ActionResult ExtendQuery(string name, int? page, int? pageSize)
        {
            var productTypes = _productTypeService.GetAllProductTypes();
            ViewBag.ProductTypes = productTypes.ToList();
            ViewBag.ExtendedQueries = _extendedQueryManager.GetExtendedQueries<Product, Product>();
            IPagedList<Product> model = null;
            var query = _extendedQueryManager.GetExtendedQuery<Product, Product>(name);
            if (query != null)
            {
                var paras = _extendedQueryManager.GetExtendedQueryParameters<Product, Product>(name);

                model = query.Query<Product>(paras, _db, page ?? 1, pageSize ?? 50, o => o);
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
            var query = _extendedQueryManager.GetExtendedQuery<Product, Product>(name);
            var paras = _extendedQueryManager.GetExtendedQueryParameters<Product, Product>(name);
            return JsonNet(new { Query = query, Parameters = paras });
        }

        [HttpPost]
        public ActionResult SaveParameters(string name, IEnumerable<ExtendedQueryParameter> parameters)
        {
            try
            {
                _extendedQueryManager.SaveExtendedQueryParameters<Product, Product>(name, parameters);
                return this.JsonNet(new { status = 0, message = "Parameter Saved." });
            }
            catch (Exception ex)
            {
                return this.JsonNet(new { status = 1, message = ex.Message });
            }
        }
    }
}
