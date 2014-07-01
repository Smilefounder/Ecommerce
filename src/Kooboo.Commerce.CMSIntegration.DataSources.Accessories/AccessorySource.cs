using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Accessories;
using Kooboo.Commerce.CMSIntegration.DataSources.Sources;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Accessories
{
    public class AccessorySource : ICommerceSource
    {
        public string Name
        {
            get
            {
                return "Accessories";
            }
        }

        public IEnumerable<SourceFilterDefinition> Filters
        {
            get
            {
                yield return new SourceFilterDefinition("ByProduct")
                {
                    Parameters = new List<FilterParameterDefinition> {
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

        public Func<IProductAccessoryService> ProductAccessoryService = () => EngineContext.Current.Resolve<IProductAccessoryService>();

        private ICommerceInstanceManager _instanceManager;

        public AccessorySource(ICommerceInstanceManager instanceManager)
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

            using (var instance = _instanceManager.GetInstance(instanceName))
            using (var scope = Scope.Begin(instance))
            {
                var accessories = ProductAccessoryService().GetAccessories((int)productId);
                if (!String.IsNullOrEmpty(context.SortField) && context.SortField == "Rank")
                {
                    if (context.SortDirection == SortDirection.Asc)
                    {
                        accessories = accessories.OrderBy(r => r.Rank).ToList();
                    }
                    else
                    {
                        accessories = accessories.OrderByDescending(r => r.Rank).ToList();
                    }
                }

                if (context.Top != null)
                {
                    accessories = accessories.Take(context.Top.Value).ToList();
                } 
                
                var accessoryIds = accessories.Select(x => x.ProductId).ToArray();

                var result = new List<Kooboo.Commerce.API.Products.Product>();
                foreach (var id in accessoryIds)
                {
                    var model = EngineContext.Current.Resolve<Kooboo.Commerce.API.Products.IProductAPI>()
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

        public IDictionary<string, object> GetDefinitions()
        {
            return DataSourceDefinitionHelper.GetDefinitions(typeof(Kooboo.Commerce.API.Products.Product));
        }
    }
}
