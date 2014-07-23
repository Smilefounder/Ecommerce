using System;
using System.Linq;
using System.Web.Mvc;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Categories.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Categories;
using Kooboo.CMS.Common;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.UI.Topbar;
using Kooboo.Commerce.Web.Areas.Commerce.Common.Topbar;
using Kooboo.Commerce.Web.Areas.Commerce.Models.TabQueries;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using Kooboo.Commerce.Web.Areas.Commerce.Common.Tabs.Queries.Products;
using Kooboo.Commerce.Web.Areas.Commerce.Common.Tabs.Queries.Products.Default;
using Kooboo.Commerce.Web.Framework.Mvc.ModelBinding;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ProductController : CommerceController
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

        public ActionResult Index(string search, string queryId, int page = 1, int pageSize = 50)
        {
            var manager = new SavedTabQueryManager();
            var model = new TabQueryModel
            {
                PageName = "Products",
                SavedQueries = manager.FindAll("Products").ToList(),
                AvailableQueries = TabQueries.GetQueries(ControllerContext).ToList()
            };

            if (model.SavedQueries.Count == 0)
            {
                var savedQuery = SavedTabQuery.CreateFrom(new DefaultProductsQuery(), "All");
                manager.Add(model.PageName, savedQuery);
                model.SavedQueries.Add(savedQuery);
            }

            if (String.IsNullOrEmpty(queryId))
            {
                model.CurrentQuery = model.SavedQueries.FirstOrDefault();
            }
            else
            {
                model.CurrentQuery = manager.Find("Products", new Guid(queryId));
            }

            var query = model.AvailableQueries.Find(q => q.Name == model.CurrentQuery.QueryName);

            model.CurrentQueryResult = query.Execute(new QueryContext(CurrentInstance, search, page - 1, pageSize, model.CurrentQuery.Config))
                                            .ToPagedList();

            ViewBag.ProductTypes = _productTypeService.Query().ToList();

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult ExecuteToolbarCommand(string commandName, ProductModel[] model, [ModelBinder(typeof(BindingTypeAwareModelBinder))]object config = null)
        {
            var command = TopbarCommands.GetCommand(commandName);
            var products = model.Select(m => _productService.GetById(m.Id)).ToList();
            var result = command.Execute(products, config, CommerceInstance.Current);
            if (result == null)
            {
                result = AjaxForm().ReloadPage();
            }

            command.UpdateDefaultCommandConfig(config);

            return result;
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

            ViewBag.Product = product;
            ViewBag.ProductType = productType;
            ViewBag.ToolbarCommands = TopbarCommands.GetCommands(ControllerContext, product, CurrentInstance);

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
