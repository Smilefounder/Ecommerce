using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.CMSIntegration
{
    public class FilterDefinition
    {
        public string Name { get; set; }

        /// <summary>
        /// Filter对应的Lucene Document中的字段，如果对应多个字段，则用半角逗号分隔，相当于查询 Field1 = FieldValue OR Field2 = FieldValue。
        /// </summary>
        public string Fields { get; set; }

        public bool SupportRangeFiltering { get; set; }

        /// <summary>
        /// 是否需要对输入字符串进行分词，对于Analyzed字段，这个属性值通常都应该是true。
        /// </summary>
        public bool AnalyzeInput { get; set; }

        public Filter CreateFilter()
        {
            return new Filter
            {
                Name = Name,
                Fields = Fields,
                AnalyzeInput = AnalyzeInput
            };
        }

        public static readonly FilterDefinition Keywords = new FilterDefinition { Name = "Keywords", Fields = "Name,SearchText", AnalyzeInput = true };

        /// <summary>
        /// 所有静态字段的过滤器定义，不包含可动态添加的Custom Field的过滤器。
        /// </summary>
        public static readonly FilterDefinition[] StaticFieldFilters = new[] {
            Keywords,
            new FilterDefinition { Name = "Name", Fields = "Name" },
            new FilterDefinition { Name = "Brand", Fields = "Brand" },
            new FilterDefinition { Name = "BrandId", Fields = "BrandId" },
            new FilterDefinition { Name = "Category", Fields = "Categories" },
            new FilterDefinition { Name = "CategoryId", Fields = "CategoryIds" },
            new FilterDefinition { Name = "LowestPrice", Fields = "LowestPrice", SupportRangeFiltering = true },
            new FilterDefinition { Name = "HighestPrice", Fields = "HighestPrice", SupportRangeFiltering = true }
        };

        public static IList<FilterDefinition> GetFilterDefinitions(IEnumerable<ProductType> productTypes)
        {
            var filters = new List<FilterDefinition>(StaticFieldFilters);

            foreach (var productType in productTypes)
            {
                foreach (var fieldDef in productType.VariantFieldDefinitions)
                {
                    if (!filters.Any(f => f.Fields == fieldDef.Name))
                    {
                        filters.Add(new FilterDefinition
                        {
                            Name = fieldDef.Name,
                            Fields = ModelConverter.GetDictionaryFieldName("VariantFieldValues", fieldDef.Name)
                        });
                    }
                }
            }

            return filters;
        }
    }
}