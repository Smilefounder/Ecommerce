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

        public string Field { get; set; }

        /// <summary>
        /// If need to lowercase the input when searching. Analyzed fields need to lowercase the input (default behavior of StandardAnalyzer).
        /// </summary>
        public bool LowercaseInput { get; set; }

        public bool SupportRangeFiltering { get; set; }

        public Filter CreateFilter()
        {
            return new Filter
            {
                Name = Name,
                Field = Field,
                LowercaseInput = LowercaseInput
            };
        }

        public static FilterDefinition Keywords = new FilterDefinition { Name = "Keywords", Field = "SearchText", LowercaseInput = true };

        public static IList<FilterDefinition> GetFilterDefinitions(IEnumerable<ProductType> productTypes)
        {
            var filters = new List<FilterDefinition>
            {
                Keywords,
                new FilterDefinition { Name = "Brand", Field = "Brand" },
                new FilterDefinition { Name = "Category", Field = "Categories" },
                new FilterDefinition { Name = "LowestPrice", Field = "LowestPrice", SupportRangeFiltering = true },
                new FilterDefinition { Name = "HighestPrice", Field = "HighestPrice", SupportRangeFiltering = true }
            };

            foreach (var productType in productTypes)
            {
                foreach (var fieldDef in productType.VariantFieldDefinitions)
                {
                    if (!filters.Any(f => f.Field == fieldDef.Name))
                    {
                        filters.Add(new FilterDefinition
                        {
                            Name = fieldDef.Name,
                            Field = ModelConverter.GetDictionaryFieldName("VariantFieldValues", fieldDef.Name)
                        });
                    }
                }
            }

            return filters;
        }
    }
}