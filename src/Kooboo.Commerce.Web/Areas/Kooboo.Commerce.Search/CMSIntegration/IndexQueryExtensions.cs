using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.CMSIntegration
{
    static class IndexQueryExtensions
    {
        public static IndexQuery ApplyFilters(this IndexQuery query, IEnumerable<Filter> parsedFilters)
        {
            foreach (var filter in parsedFilters)
            {
                if (filter.UseRangeFiltering)
                {
                    var fromValue = ModelConverter.ParseFieldValue(query.ModelType, filter.Field, filter.FromValue);
                    var toValue = ModelConverter.ParseFieldValue(query.ModelType, filter.Field, filter.ToValue);

                    query = query.WhereBetween(filter.Field, fromValue, toValue, filter.FromInclusive, filter.ToInclusive);
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(filter.FieldValue))
                    {
                        continue;
                    }

                    var filterValue = ModelConverter.ParseFieldValue(query.ModelType, filter.Field, filter.FieldValue);
                    if (filterValue != null)
                    {
                        query = query.WhereEquals(filter.Field, filterValue);
                    }
                }
            }

            return query;
        }
    }
}