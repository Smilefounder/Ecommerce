using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.CMSIntegration.DataSources.Generic;
using Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Recommendations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Metadata;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Recommendations
{
    [DataContract]
    [KnownType(typeof(RecommendationDataSource))]
    public class RecommendationDataSource : GenericCommerceDataSource
    {
        public override string Name
        {
            get
            {
                return "Recommendations";
            }
        }

        public override IEnumerable<FilterDescription> Filters
        {
            get
            {
                return new[] { new FilterDescription("ByProduct", new Int32ParameterDescription("ProductId")) };
            }
        }

        public override IEnumerable<string> SortFields
        {
            get
            {
                yield return "Rank";
            }
        }

        public override IEnumerable<string> OptionalIncludeFields
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

        public RecommendationDataSource(ICommerceInstanceManager instanceManager)
        {
            _instanceManager = instanceManager;
        }

        protected override object DoExecute(CommerceDataSourceContext context, ParsedGenericCommerceDataSourceSettings settings)
        {
            var productIdFilter = settings.Filters.Find(f => f.Name == "ByProduct");
            if (productIdFilter == null)
            {
                return null;
            }

            var productId = productIdFilter.GetParameterValue("productId");
            if (productId == null)
            {
                return null;
            }

            var instanceName = context.Site.GetCommerceInstanceName();

            if (String.IsNullOrWhiteSpace(instanceName))
                throw new InvalidOperationException("Commerce instance name is not configured in CMS.");

            using (var instance = _instanceManager.GetInstance(instanceName))
            using (var scope = Scope.Begin(instance))
            {
                var recommendations = ProductRecommendationService().GetRecommendations((int)productId);
                if (!String.IsNullOrEmpty(settings.SortField) && settings.SortField == "Rank")
                {
                    if (settings.SortDirection == SortDirection.Asc)
                    {
                        recommendations = recommendations.OrderBy(r => r.Rank).ToList();
                    }
                    else
                    {
                        recommendations = recommendations.OrderByDescending(r => r.Rank).ToList();
                    }
                }

                if (settings.Top != null)
                {
                    recommendations = recommendations.Take(settings.Top.Value).ToList();
                }

                var productIds = recommendations.Select(x => x.ProductId).ToArray();

                var result = new List<Kooboo.Commerce.Api.Products.Product>();
                foreach (var id in productIds)
                {
                    var model = EngineContext.Current.Resolve<Kooboo.Commerce.Api.Products.IProductApi>()
                                           .Query()
                                           .ById(id)
                                           .Include("PriceList")
                                           .Include("Images")
                                           .Include("Brand")
                                           .FirstOrDefault();
                    if (model != null)
                    {
                        result.Add(model);
                    }
                }

                return result;
            }
        }

        public override IDictionary<string, object> GetDefinitions(CommerceDataSourceContext context)
        {
            return DataSourceDefinitionHelper.GetDefinitions(typeof(Kooboo.Commerce.Api.Products.Product));
        }
    }
}
