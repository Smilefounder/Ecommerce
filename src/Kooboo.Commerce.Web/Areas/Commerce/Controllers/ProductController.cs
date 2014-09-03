using System;
using System.Linq;
using System.Web.Mvc;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Categories;
using Kooboo.CMS.Common;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.UI.Topbar;
using Kooboo.Commerce.Web.Areas.Commerce.Topbar;
using Kooboo.Commerce.Web.Areas.Commerce.Models.TabQueries;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Products;
using Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Products.Default;
using Kooboo.Commerce.Web.Framework.Mvc.ModelBinding;
using AutoMapper;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Products;
using Kooboo.Commerce.Web.Areas.Commerce.Models;
using System.Collections.Generic;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ProductController : CommerceController
    {
        private SettingService _settingService;
        private ProductService _productService;
        private ProductTypeService _productTypeService;
        private BrandService _brandService;
        private CategoryService _categoryService;

        public ProductController(
                SettingService settingService,
                ProductService productService,
                ProductTypeService productTypeService,
                BrandService brandService,
                CategoryService categoryService)
        {
            _settingService = settingService;
            _productService = productService;
            _productTypeService = productTypeService;
            _brandService = brandService;
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            var model = this.CreateTabQueryModel("Products", new DefaultProductsQuery());
            ViewBag.ProductTypes = _productTypeService.Query().ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int productTypeId)
        {
            var productType = _productTypeService.GetById(productTypeId);

            ViewBag.ProductType = productType;
            ViewBag.DefaultVariantModel = CreateDefaultVariantModel(productType);
            ViewBag.ImageTypes = _settingService.Get<GlobalSettings>().Image.Types;

            this.LoadTabPlugins();

            return View("Edit");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = _productService.GetById(id);
            var productType = product.ProductType;

            ViewBag.Product = product;
            ViewBag.ProductType = productType;
            ViewBag.DefaultVariantModel = CreateDefaultVariantModel(productType);
            ViewBag.ImageTypes = _settingService.Get<GlobalSettings>().Image.Types;
            ViewBag.ToolbarCommands = TopbarCommands.GetCommands(ControllerContext, product, CurrentInstance);

            this.LoadTabPlugins();

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
                product = new Product
                {
                    ProductType = _productTypeService.GetById(productTypeId.Value)
                };
            }

            var model = Mapper.Map<Product, ProductEditorModel>(product);

            return JsonNet(model).UsingClientConvention();
        }

        [HttpPost]
        public ActionResult Save(ProductEditorModel model)
        {
            var product = DoSave(model);
            return JsonNet(new
            {
                Id = product.Id
            }).UsingClientConvention();
        }

        private Product DoSave(ProductEditorModel model)
        {
            Product product = null;

            if (model.Id == 0)
            {
                product = new Product
                {
                    ProductType = _productTypeService.GetById(model.ProductTypeId)
                };
                UpdateProduct(product, model);
                _productService.Create(product);
            }
            else
            {
                product = _productService.GetById(model.Id);
                UpdateProduct(product, model);
                _productService.Update(product);
            }

            if (model.IsPublished)
            {
                _productService.Publish(product);
            }
            else
            {
                _productService.Unpublish(product);
            }

            return product;
        }

        private void UpdateProduct(Product product, ProductEditorModel model)
        {
            product.Name = model.Name;
            product.Brand = model.Brand == null ? null : _brandService.GetById(model.Brand.Id);

            product.Categories.Clear();
            foreach (var category in model.Categories)
            {
                product.Categories.Add(_categoryService.GetById(category.Id));
            }

            product.SetCustomFields(model.CustomFields);
            product.SetImages(model.Images);

            foreach (var variant in product.Variants.ToList())
            {
                if (!model.Variants.Any(v => v.Id == variant.Id))
                {
                    product.Variants.Remove(variant);
                }
            }

            foreach (var variantModel in model.Variants)
            {
                ProductVariant variant;

                if (variantModel.Id > 0)
                {
                    variant = product.Variants.FirstOrDefault(v => v.Id == variantModel.Id);
                }
                else
                {
                    variant = new ProductVariant();
                    product.Variants.Add(variant);
                }

                variant.Sku = variantModel.Sku;
                variant.Price = variantModel.Price;
                variant.SetVariantFields(variantModel.VariantFields);
            }
        }

        public ProductVariantModel CreateDefaultVariantModel(ProductType productType)
        {
            var variant = new ProductVariantModel();
            foreach (var fieldDef in productType.VariantFieldDefinitions)
            {
                variant.VariantFields.Add(fieldDef.Name, null);
            }

            return variant;
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
                                       .Where(p => p.CustomFields.Any(f => f.FieldName == fieldName && f.FieldValue == fieldValue));

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
            var settings = _settingService.Get<GlobalSettings>();
            var sizes = settings.Image.Types.ToList();
            return JsonNet(sizes);
        }

        [HttpGet]
        public ActionResult GetProductType(int id)
        {
            var obj = _productTypeService.GetById(id);
            return JsonNet(obj);
        }

        [HttpGet]
        public ActionResult SearchBrands(string term, int page, int pageSize = 10)
        {
            var query = _brandService.Query();

            if (!String.IsNullOrWhiteSpace(term))
            {
                query = query.Where(p => p.Name.Contains(term));
            }

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);

            var items = query.OrderBy(p => p.Name)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .Select(p => new IdName
                             {
                                 Id = p.Id,
                                 Name = p.Name
                             })
                             .ToList();

            return JsonNet(new
            {
                Items = items,
                More = page < totalPages
            })
            .UsingClientConvention();
        }

        [HttpGet]
        public ActionResult SearchCategories(string term)
        {
            var all = _categoryService.Query().Select(c => new CategoryEntry
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId
            });

            var categories = CategoryEntry.BuildCategoryTree(null, all);

            return JsonNet(new
            {
                Items = categories,
                More = false
            })
            .UsingClientConvention();
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
