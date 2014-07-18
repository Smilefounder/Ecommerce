using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Recommendations.Controllers
{
    public class RecommendationController : CommerceController
    {
        private IProductService _productService;

        public RecommendationController(IProductService productService)
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
