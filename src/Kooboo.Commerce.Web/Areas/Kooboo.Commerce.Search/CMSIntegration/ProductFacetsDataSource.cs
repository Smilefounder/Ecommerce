using Kooboo.Commerce.CMSIntegration.DataSources.Sources;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using Kooboo.Commerce.CMSIntegration;

namespace Kooboo.Commerce.Search.CMSIntegration
{
    public class ProductFacetsDataSource : ICommerceSource
    {
        public string Name
        {
            get
            {
                return "ProductFacets";
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
            return null;
        }

        public IDictionary<string, object> GetDefinitions()
        {
            return new Dictionary<string, object>();
        }
    }
}
