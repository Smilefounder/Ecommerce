using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.CMSIntegration.DataSources.Sources;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Recommendations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Recommendations
{
    [Dependency(typeof(ICommerceSource), Key = "Recommendations")]
    public class RecommendationSource : ICommerceSource
    {
        public string Name
        {
            get
            {
                return "Recommendations";
            }
        }

        public IEnumerable<SourceFilterDefinition> Filters
        {
            get
            {
                yield return new SourceFilterDefinition("ByProduct")
                {
                    Parameters = new List<FilterParameterDefinition>{
                        new FilterParameterDefinition("productId", typeof(Int32))
                    }
                };
            }
        }

        public IEnumerable<string> SortableFields
        {
            get
            {
                return null;
            }
        }

        public IEnumerable<string> IncludablePaths
        {
            get
            {
                return null;
            }
        }

        // Cannot inject IProductService instance to this class
        // because ICommerceSource will be resolved in CMS data source management page
        // where there's no a commerce instance context
        // while IProductService requires a commerce instance context.
        public Func<IProductService> ProductService = () => EngineContext.Current.Resolve<IProductService>();

        public Func<IProductRecommendationService> ProductRecommendationService = () => EngineContext.Current.Resolve<IProductRecommendationService>();

        private ICommerceInstanceManager _instanceManager;

        public RecommendationSource(ICommerceInstanceManager instanceManager)
        {
            _instanceManager = instanceManager;
        }

        public object Execute(CommerceSourceContext context)
        {
            var productIdFilter = context.Filters.Find(f => f.Name == "ByProduct");
            if (productIdFilter == null)
            {
                return null;
            }

            var productId = productIdFilter.GetParameterValue("productId");
            if (productId == null)
            {
                return null;
            }

             var instanceName = context.DataSourceContext.Site.CommerceInstanceName();

            if (String.IsNullOrWhiteSpace(instanceName))
                throw new InvalidOperationException("Commerce instance name is not configured in CMS.");

            using (var instance = _instanceManager.OpenInstance(instanceName))
            using (var scope = Scope.Begin(instance))
            {
                var recommendations = ProductRecommendationService().GetRecommendations((int)productId);
                var productIds = recommendations.Select(x => x.ProductId).ToArray();
                var products = ProductService().Query()
                                               .Where(x => productIds.Contains(x.Id))
                                               .ToList();

                return products;
            }
        }
    }
}
