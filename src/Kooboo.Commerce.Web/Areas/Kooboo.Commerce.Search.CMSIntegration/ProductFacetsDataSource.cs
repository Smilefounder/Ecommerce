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
            var api = new FacetsApi(GetCommerceHost(context), context.Site.GetCommerceInstanceName());
            return api.Facets(context.Site.Culture);
        }

        private string GetCommerceHost(CommerceSourceContext context)
        {
            var host = context.Site.GetCommerceApiHost();
            if (String.IsNullOrEmpty(host))
            {
                var httpRequest = HttpContext.Current.Request;
                host = httpRequest.Url.Scheme + "://" + httpRequest.Url.Authority;
            }

            return host;
        }

        public IDictionary<string, object> GetDefinitions()
        {
            return new Dictionary<string, object>();
        }
    }
}
