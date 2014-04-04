using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Recommendations.Models;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Recommendations.Controllers
{
    public class RecommendationController : CommerceControllerBase
    {
        private IProductService _productService;
        private IProductRecommendationService _recommendationService;

        public RecommendationController(
            IProductService productService,
            IProductRecommendationService recommendationService)
        {
            _productService = productService;
            _recommendationService = recommendationService;
        }

        public ActionResult List(int productId)
        {
            var recommendations = _recommendationService.GetRecommendations(productId);
            var models = new List<ProductRecommendationModel>();

            foreach (var recommendation in recommendations)
            {
                var product = _productService.GetById(recommendation.ProductId);
                if (product != null)
                {
                    var model = new ProductRecommendationModel
                    {
                        ProductId = recommendation.ProductId,
                        ProductName = product.Name,
                        BrandName = product.Brand.Name,
                        Rank = recommendation.Rank
                    };

                    models.Add(model);
                }
            }

            return JsonNet(models).UsingClientConvention();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Save(int productId, IList<ProductRecommendationModel> recommendations)
        {
            _recommendationService.SaveRecommendations(productId, recommendations.Select(x => new ProductRecommendation
            {
                ProductId = x.ProductId,
                Rank = x.Rank
            }));

            return AjaxForm();
        }

        public ActionResult SearchProducts(string term)
        {
            var products = _productService.Query()
                                          .Where(x => !x.IsDeleted && x.Name.Contains(term))
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
