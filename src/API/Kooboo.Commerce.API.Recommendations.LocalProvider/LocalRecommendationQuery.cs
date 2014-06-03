using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.HAL;
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
    [Dependency(typeof(IRecommendationQuery))]
    public class LocalRecommendationQuery : LocalCommerceQuery<Product, Kooboo.Commerce.Products.Product>, IRecommendationQuery
    {
        private IProductService _productService;
        private IProductRecommendationService _recommendationService;

        public LocalRecommendationQuery(
            IHalWrapper halWrapper,
            IProductService productService,
            IProductRecommendationService recommendationService,
            IMapper<Product, Kooboo.Commerce.Products.Product> mapper)
            : base(halWrapper, mapper)
        {
            _productService = productService;
            _recommendationService = recommendationService;
        }

        protected override IQueryable<Commerce.Products.Product> CreateQuery()
        {
            return _productService.Query();
        }

        protected override IQueryable<Commerce.Products.Product> OrderByDefault(IQueryable<Commerce.Products.Product> query)
        {
            return query;
        }

        public IRecommendationQuery ByProduct(int productId)
        {
            EnsureQuery();
            var recommendations = _recommendationService.GetRecommendations(productId);
            var productIds = recommendations.Select(x => x.ProductId).ToArray();
            _query = _productService.Query().Where(x => productIds.Contains(x.Id));
            return this;
        }
    }
}
