using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.LocalProvider;
using Kooboo.Commerce.API.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Recommendations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Recommendations.LocalProvider
{
    [Dependency(typeof(IRecommendationAPI))]
    public class LocalRecommendationAPI : IRecommendationAPI
    {
        private IProductService _productService;
        private IProductRecommendationService _recommendationService;
        private IMapper<Product, Kooboo.Commerce.Products.Product> _mapper;

        public LocalRecommendationAPI(
            IProductService productService,
            IProductRecommendationService recommendationService,
            IMapper<Product, Kooboo.Commerce.Products.Product> mapper)
        {
            _productService = productService;
            _recommendationService = recommendationService;
            _mapper = mapper;
        }

        public Product[] ForProduct(int productId)
        {
            var recommendations = _recommendationService.GetRecommendations(productId);
            var productIds = recommendations.Select(x => x.ProductId).ToArray();
            var products = _productService.Query()
                                          .Where(x => productIds.Contains(x.Id))
                                          .ToList();

            return products.Select(x => _mapper.MapTo(x)).ToArray();
        }
    }
}
