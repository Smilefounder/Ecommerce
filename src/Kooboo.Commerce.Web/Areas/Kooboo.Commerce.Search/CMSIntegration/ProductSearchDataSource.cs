using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.CMSIntegration;
using Kooboo.Commerce.CMSIntegration.DataSources;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Search.Facets;
using Kooboo.Commerce.Search.Models;
using Kooboo.Commerce.Text;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.Search.CMSIntegration
{
    [DataContract]
    [KnownType(typeof(ProductSearchDataSource))]
    public class ProductSearchDataSource : ICommerceDataSource
    {
        public string Name
        {
            get { return "ProductSearch"; }
        }

        public string EditorVirtualPath
        {
            get
            {
                return "~/Areas/" + Strings.AreaName + "/Views/DataSource/ProductSearch.cshtml";
            }
        }

        [DataMember]
        public IList<Filter> Filters { get; set; }

        [DataMember]
        public bool IncludeFacets { get; set; }

        [DataMember]
        public IList<Facet> Facets { get; set; }

        /// <summary>
        /// The field used for sorting. Use + prefix for asc sorting, and use - prefix for desc sorting.
        /// </summary>
        [DataMember]
        public string SortField { get; set; }

        [DataMember]
        public string PageSize { get; set; }

        [DataMember]
        public string PageNumber { get; set; }

        public ProductSearchDataSource()
        {
            var keywordsFilter = FilterDefinition.Keywords.CreateFilter();
            keywordsFilter.FieldValue = "{keywords}";

            Filters = new List<Filter>
            {
                keywordsFilter
            };

            Facets = new List<Facet>();

            PageSize = "{pageSize}";
            PageNumber = "{page}";
        }

        public object Execute(CommerceDataSourceContext context)
        {
            var culture = CultureInfo.InvariantCulture;

            if (!String.IsNullOrEmpty(context.Site.Culture))
            {
                var languageStore = LanguageStores.Get(context.Instance);
                if (languageStore.Exists(context.Site.Culture))
                {
                    culture = CultureInfo.GetCultureInfo(context.Site.Culture);
                }
            }

            var store = IndexStores.Get<ProductModel>(context.Instance, culture);
            var query = store.Query();

            // Apply defined filters
            if (Filters != null)
            {
                query = query.ApplyFilters(ParseFilters(context), culture);
            }

            // Apply implicit facet filters repsented in url query
            IList<Filter> facetFilters = null;
            if (IncludeFacets && Facets != null && Facets.Count > 0)
            {
                facetFilters = GetFacetFilters(context);
                if (facetFilters.Count > 0)
                {
                    query = query.ApplyFilters(facetFilters, culture);
                }
            }

            // Apply sorting
            var sortField = ParameterizedFieldValue.GetFieldValue(SortField, context.ValueProvider);
            if (!String.IsNullOrEmpty(sortField))
            {
                query.AddOrderBy(sortField);
            }

            // Execute result
            var result = new ProductSearchResult();

            var pagination = query.Paginate(EvaluatePageNumber(context) - 1, EvaluatePageSize(context));

            result.Total = pagination.TotalItems;
            result.Products = ToProducts(pagination.Cast<ProductModel>().ToList(), context.Site.Commerce());

            // Get facets
            if (IncludeFacets && Facets != null && Facets.Count > 0)
            {
                result.Facets = query.ToFacets(Facets);
                
                // Generate facet urls
                var url = HttpContext.Current.Request.RawUrl;

                CategoryTree localizedCategoryTree = null;
                
                foreach (var facetResult in result.Facets)
                {
                    // Categories are in tree structure, and all level of categories of a product are indexed for seaier search.
                    // If Category facet is not presented in url query, we should only return the ROOT level categories.
                    // If Category facet is presented in url query, we should only return it's children categories.
                    // Otherwise there'll be too many category results.
                    if (facetResult.Field == FacetDefinition.Category.Field)
                    {
                        if (localizedCategoryTree == null)
                        {
                            localizedCategoryTree = CategoryTree.Get(context.Instance).Localize(culture);
                        }

                        var filter = facetFilters.FirstOrDefault(f => f.Name == FacetDefinition.Category.Name);

                        if (filter != null)
                        {
                            var category = localizedCategoryTree.Desendants().FirstOrDefault(c => c.Name == filter.FieldValue);
                            foreach (var value in facetResult.Values.ToList())
                            {
                                if (!category.Children.Any(c => c.Name == value.Term))
                                {
                                    facetResult.Values.Remove(value);
                                }
                            }
                        }
                        else
                        {
                            foreach (var value in facetResult.Values.ToList())
                            {
                                if (!localizedCategoryTree.Categories.Any(c => c.Name == value.Term))
                                {
                                    facetResult.Values.Remove(value);
                                }
                            }
                        }
                    }

                    foreach (var value in facetResult.Values)
                    {
                        var paramName = facetResult.Name;
                        value.Url = UrlUtility.RemoveQuery(url, paramName);
                        value.Url = UrlUtility.AddQueryParam(value.Url, paramName, value.Term);
                    }
                }
            }

            return result;
        }

        private IList<Filter> GetFacetFilters(CommerceDataSourceContext context)
        {
            var facetFilters = new List<Filter>();
            foreach (var facet in Facets)
            {
                // Ignore if the filter is already defined
                if (Filters != null && Filters.Any(f => f.Name == facet.Name))
                {
                    continue;
                }

                var value = context.ValueProvider.GetValue(facet.Name);
                if (value != null && !String.IsNullOrEmpty(value.AttemptedValue))
                {
                    var filterValue = value.AttemptedValue;
                    var filter = new Filter
                    {
                        Name = facet.Name,
                        Field = facet.Field,
                        FieldValue = filterValue
                    };

                    if (facet.Ranges != null && facet.Ranges.Count > 0)
                    {
                        // If it's a range facet, then try to find range by label.
                        // If nothing was found, then the range label might be empty, so parse it with range syntax.
                        var range = facet.Ranges.FirstOrDefault(r => !String.IsNullOrEmpty(r.Label) && r.Label.Equals(filterValue));
                        if (range == null)
                        {
                            range = FacetRange.Parse(null, filterValue);
                        }

                        filter.UseRangeFiltering = true;
                        filter.FromValue = range.FromValue.ToString();
                        filter.ToValue = range.ToValue.ToString();
                        filter.FromInclusive = range.FromInclusive;
                        filter.ToInclusive = range.ToInclusive;
                    }

                    facetFilters.Add(filter);
                }
            }

            return facetFilters;
        }

        private List<Product> ToProducts(IList<ProductModel> models, ICommerceApi api)
        {
            var productIds = models.Select(m => m.Id).ToArray();
            var products = api.Products.Query()
                                       .ByIds(productIds)
                                       .Include(p => p.Images)
                                       .Include(p => p.Variants)
                                       .ToList();

            var result = new List<Product>();
            foreach (var id in productIds)
            {
                var product = products.Find(p => p.Id == id);
                if (product != null)
                {
                    result.Add(product);
                }
            }

            return result;
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
            return false;
        }
    }
}