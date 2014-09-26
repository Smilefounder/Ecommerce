using Lucene.Net.QueryParsers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.CMSIntegration
{
    static class IndexQueryExtensions
    {
        public static IndexQuery ApplyFilters(this IndexQuery query, IEnumerable<Filter> parsedFilters, CultureInfo culture)
        {
            foreach (var filter in parsedFilters)
            {
                if (filter.UseRangeFiltering)
                {
                    var fromValue = ModelConverter.ParseFieldValue(query.ModelType, filter.Fields, filter.FromValue);
                    var toValue = ModelConverter.ParseFieldValue(query.ModelType, filter.Fields, filter.ToValue);

                    query = query.WhereBetween(filter.Fields, fromValue, toValue, filter.FromInclusive, filter.ToInclusive);
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(filter.FieldValue))
                    {
                        continue;
                    }

                    // 如果需要分析输入，则需要调用Search方法。动态的Custom Field字段都不用分析输入，其实只有SearchText字段才需要分析输入
                    if (filter.AnalyzeInput)
                    {
                        // 如果一个Filter对应了多个字段，则要同时对它们进行搜索，然后做OR操作
                        foreach (var field in filter.GetFields())
                        {
                            query = query.Or(_ => _.Search(field, filter.FieldValue));
                        }
                    }
                    else
                    {
                        var filterValue = ModelConverter.ParseFieldValue(query.ModelType, filter.Fields, filter.FieldValue);
                        if (filterValue != null)
                        {
                            foreach (var field in filter.GetFields())
                            {
                                query = query.Or(_ => _.WhereEquals(field, filterValue));
                            }
                        }
                    }
                }
            }

            return query;
        }
    }
}