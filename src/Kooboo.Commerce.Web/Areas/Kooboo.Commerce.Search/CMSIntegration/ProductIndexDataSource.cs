using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.Commerce.CMSIntegration.DataSources;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Search.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace Kooboo.Commerce.Search.CMSIntegration
{
    [DataContract]
    [KnownType(typeof(ProductIndexDataSource))]
    public class ProductIndexDataSource : ICommerceDataSource
    {
        public string Name
        {
            get { return "ProductIndexes"; }
        }

        public string EditorVirtualPath
        {
            get
            {
                return "~/Areas/" + Strings.AreaName + "/Views/DataSource/ProductIndexes.cshtml";
            }
        }

        [DataMember]
        public IList<Filter> Filters { get; set; }

        /// <summary>
        /// The field used for sorting. Use + prefix for asc sorting, and use - prefix for desc sorting.
        /// </summary>
        [DataMember]
        public string SortField { get; set; }

        [DataMember]
        public string PageSize { get; set; }

        [DataMember]
        public string PageNumber { get; set; }

        public ProductIndexDataSource()
        {
            Filters = new List<Filter>();
        }

        public object Execute(CommerceDataSourceContext context)
        {
            var culture = CultureInfo.InvariantCulture;

            if (!String.IsNullOrEmpty(context.Site.Culture))
            {
                var languageStore = EngineContext.Current.Resolve<ILanguageStore>();
                if (languageStore.Exists(context.Site.Culture))
                {
                    culture = CultureInfo.GetCultureInfo(context.Site.Culture);
                }
            }

            var store = IndexStores.Get<ProductModel>(context.Instance, culture);
            var query = store.Query();

            // Apply filters
            if (Filters != null)
            {
                query = query.ApplyFilters(ParseFilters(context), culture);
            }

            // Apply sorting
            var sortField = ParameterizedFieldValue.GetFieldValue(SortField, context.ValueProvider);
            if (!String.IsNullOrEmpty(sortField))
            {
                query.AddOrderBy(sortField);
            }

            return query.Paginate(EvaluatePageNumber(context) - 1, EvaluatePageSize(context));
        }

        private List<Filter> ParseFilters(CommerceDataSourceContext context)
        {
            return Filters.Select(f => f.Parse(context.ValueProvider)).ToList();
        }

        private int EvaluatePageSize(CommerceDataSourceContext context)
        {
            var pageSize = ParameterizedFieldValue.GetFieldValue(PageSize, context.ValueProvider);
            if (String.IsNullOrEmpty(pageSize))
            {
                return 60;
            }

            return Convert.ToInt32(pageSize);
        }

        private int EvaluatePageNumber(CommerceDataSourceContext context)
        {
            var pageNumber = ParameterizedFieldValue.GetFieldValue(PageNumber, context.ValueProvider);
            if (String.IsNullOrEmpty(pageNumber))
            {
                return 1;
            }

            var page = Convert.ToInt32(pageNumber);
            if (page < 1)
            {
                page = 1;
            }

            return page;
        }

        public IDictionary<string, object> GetDefinitions(CommerceDataSourceContext context)
        {
            return new Dictionary<string, object>();
        }

        public IEnumerable<string> GetParameters()
        {
            return Enumerable.Empty<string>();
        }

        public bool IsEnumerable()
        {
            return true;
        }
    }
}