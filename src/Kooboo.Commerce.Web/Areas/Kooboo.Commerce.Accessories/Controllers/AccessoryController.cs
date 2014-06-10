using Kooboo.Commerce.Products.Accessories.Models;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Accessories.Controllers
{
    public class AccessoryController : CommerceControllerBase
    {
        private IProductService _productService;
        private IProductAccessoryService _accessoryService;

        public AccessoryController(
            IProductAccessoryService accessoryService,
            IProductService productService)
        {
            _accessoryService = accessoryService;
            _productService = productService;
        }

        public ActionResult List(int productId)
        {
            var accessories = _accessoryService.GetAccessories(productId);
            var models = new List<ProductAccessoryModel>();

            foreach (var accessory in accessories)
            {
                var product = _productService.GetById(accessory.ProductId);

                if (product != null)
                {
                    var model = new ProductAccessoryModel
                    {
                        ProductId = accessory.ProductId,
                        ProductName = product.Name,
                        BrandName = product.Brand.Name,
                        Rank = accessory.Rank
                    };

                    models.Add(model);
                }
            }

            return JsonNet(models).UsingClientConvention();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Save(int productId, IList<ProductAccessoryModel> accessories)
        {
            if (accessories != null)
            {
                _accessoryService.UpdateAccessories(productId, accessories.Select(x => new ProductAccessory
                {
                    ProductId = x.ProductId,
                    Rank = x.Rank
                }));
            }

            return AjaxForm();
        }

        public ActionResult SearchProducts(string term)
        {
            var products = _productService.Query()
                                          .Where(x => x.Name.Contains(term))
                                          .Take(10)
                                          .Select(x => new
                                          {
                                              x.Id,
                                              x.Name,
                                              BrandName = x.Brand.Name
                                          })
                                          .ToList();

            return JsonNet(products).UsingClientConvention();
        }
    }
}
