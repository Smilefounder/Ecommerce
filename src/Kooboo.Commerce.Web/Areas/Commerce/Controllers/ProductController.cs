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

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{

    public class ProductController : CommerceControllerBase
    {

        private readonly IProductService _productService;
        private readonly ISettingService _settingService;
        private readonly IProductTypeService _productTypeService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;

        public ProductController(
                IProductService productService,
                ISettingService settingService,
                IProductTypeService productTypeService,
                IBrandService brandService,
                ICategoryService categoryService)
        {
            _productService = productService;
            _settingService = settingService;
            _productTypeService = productTypeService;
            _brandService = brandService;
            _categoryService = categoryService;
        }

        public ActionResult Index(string search, int? page, int? pageSize)
        {
            var productTypes = _productTypeService.GetAllProductTypes();
            var model = _productService.GetAllProducts(search, null, page, pageSize);
            ViewBag.ProductTypes = productTypes.ToList();
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
                _productService.Update(obj);
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
            var objs = new List<ImageSize>();
            var imageSett = _settingService.GetImageSetting();
            if (imageSett.Thumbnail != null)
            {
                objs.Add(imageSett.Thumbnail);
            }
            if (imageSett.Detail != null)
            {
                objs.Add(imageSett.Detail);
            }
            if (imageSett.List != null)
            {
                objs.Add(imageSett.List);
            }
            if (imageSett.Cart != null)
            {
                objs.Add(imageSett.Cart);
            }
            if (imageSett.CustomSizes != null)
            {
                objs.AddRange(imageSett.CustomSizes);
            }
            objs = objs.Where(o => o.IsEnabled).ToList();
            return JsonNet(objs);
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

            var objs = _brandService.GetAllBrands();
            return JsonNet(objs);
        }

        [HttpGet]
        public ActionResult GetCategories(int? parentId = null)
        {

            var objs = parentId.HasValue ? _categoryService.GetChildCategories(parentId.Value) : _categoryService.GetRootCategories();
            return JsonNet(objs);
        }
    }
}
