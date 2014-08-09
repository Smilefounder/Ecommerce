using Kooboo.Commerce.Data;
using Kooboo.Commerce.Search.Facets;
using Lucene.Net.Index;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search
{
    public class IndexQuery
    {
        private Query _query = new MatchAllDocsQuery();
        private IndexSearcher _searcher;

        public Type ModelType { get; private set; }

        public IndexQuery(Type modelType, IndexSearcher searcher)
        {
            ModelType = modelType;
            _searcher = searcher;
        }

        public IndexQuery WhereEquals(string field, object value)
        {
            return And(CreateTermQuery(field, value));
        }

        public IndexQuery WhereLessThan(string field, object value)
        {
            return WhereBetween(field, null, value, false, false);
        }

        public IndexQuery WhereLessThanOrEqual(string field, object value)
        {
            return WhereBetween(field, null, value, false, true);
        }

        public IndexQuery WhereGreaterThan(string field, object value)
        {
            return WhereBetween(field, value, null, false, false);
        }

        public IndexQuery WhereGreaterThanOrEqual(string field, object value)
        {
            return WhereBetween(field, value, null, true, false);
        }

        public IndexQuery WhereBetween(string field, object fromValue, object toValue, bool fromInclusive, bool toInclusive)
        {
            if (fromValue == null && toValue == null)
            {
                return this;
            }

            var isNumericField = false;

            var property = ModelType.GetProperty(field, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            // property might be null because it might be custom fields
            if (property != null)
            {
                var fieldAttr = property.GetCustomAttribute<FieldAttribute>();
                isNumericField = fieldAttr != null && fieldAttr.Numeric;
            }

            Query query = null;

            if (isNumericField)
            {
                query = CreateNumericRangeQuery(field, property.PropertyType, fromValue, toValue, fromInclusive, toInclusive);
            }
            else
            {
                query = new TermRangeQuery(field, IndexUtil.ToFieldStringValue(fromValue), IndexUtil.ToFieldStringValue(toValue), fromInclusive, toInclusive);
            }

            return And(query);
        }

        public Pagination Paginate(int pageIndex, int pageSize)
        {
            var docs = _searcher.Search(_query, null, Int32.MaxValue);

            var items = new List<object>();
            var start = pageIndex * pageSize;
            var bound = start + pageSize;
            if (bound > docs.TotalHits)
            {
                bound = docs.TotalHits;
            }

            for (var i = start; i < bound; i++)
            {
                var doc = docs.ScoreDocs[i];
                items.Add(ModelConverter.ToModel(_searcher.Doc(doc.Doc), ModelType));
            }

            return new Pagination(items, pageIndex, pageSize, docs.TotalHits);
        }

        public IList<FacetResult> ToFacets(IEnumerable<Facet> facets)
        {
            if (facets == null || !facets.Any())
            {
                return new List<FacetResult>();
            }

            var searcher = new FacetedSearcher(_searcher);
            return searcher.Search(_query, facets);
        }

        private Query CreateNumericRangeQuery(string property, Type propType, object fromValue, object toValue, bool minInclusive, bool maxInclusive)
        {
            if (propType == typeof(int))
            {
                return NumericRangeQuery.NewIntRange(property, (int?)fromValue, (int?)toValue, minInclusive, maxInclusive);
            }
            if (propType == typeof(long))
            {
                return NumericRangeQuery.NewLongRange(property, (long?)fromValue, (long?)toValue, minInclusive, maxInclusive);
            }
            if (propType == typeof(float))
            {
                return NumericRangeQuery.NewFloatRange(property, (float?)fromValue, (float?)toValue, minInclusive, maxInclusive);
            }

            return NumericRangeQuery.NewDoubleRange(property, fromValue == null ? null : (double?)Convert.ToDouble(fromValue), toValue == null ? null : (double?)Convert.ToDouble(toValue), minInclusive, maxInclusive);
        }

        private TermQuery CreateTermQuery(string field, object value)
        {
            return new TermQuery(new Term(field, IndexUtil.ToFieldStringValue(value)));
        }

        private IndexQuery And(Query query)
        {
            if (query == null)
            {
                return this;
            }

            if (_query is MatchAllDocsQuery)
            {
                _query = query;
            }
            else
            {
                var newQuery = new BooleanQuery();
                newQuery.Add(_query, Occur.MUST);
                newQuery.Add(query, Occur.MUST);

                _query = newQuery;
            }

            return this;
        }
    }
}