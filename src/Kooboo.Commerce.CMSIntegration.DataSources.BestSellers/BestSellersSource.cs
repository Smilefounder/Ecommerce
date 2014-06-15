using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.CMSIntegration.DataSources.Sources;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.ExtendedQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.DataSources.BestSellers
{
    [Dependency(typeof(ICommerceSource), Key = "BestSellers")]
    public class BestSellersSource : ICommerceSource
    {
        public string Name
        {
            get
            {
                return "BestSellers";
            }
        }

        public IEnumerable<SourceFilterDefinition> Filters
        {
            get
            {
                return null;
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

        public object Execute(CommerceSourceContext context)
        {
            var query = new TopSoldProduct();
            var paras = new List<ExtendedQueryParameter>();
            var pageIndex = context.PageNumber.GetValueOrDefault(1) - 1;
            var pageSize = context.PageSize.GetValueOrDefault(10);

            return query.Query<Product>(paras, EngineContext.Current.Resolve<ICommerceDatabase>(), pageIndex, pageSize, p => p).ToList(); ;
        }
    }
}
