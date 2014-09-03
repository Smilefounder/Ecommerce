using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Accessories.Controllers
{
    public class AccessoryController : CommerceController
    {
        private ProductService _productService;

        public AccessoryController(ProductService productService)
        {
            _productService = productService;
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
