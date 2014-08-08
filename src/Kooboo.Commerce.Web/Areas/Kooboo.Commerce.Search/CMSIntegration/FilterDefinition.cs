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

        public bool SupportRangeFiltering { get; set; }

        public static IList<FilterDefinition> GetFilterDefinitions(IEnumerable<ProductType> productTypes)
        {
            var filters = new List<FilterDefinition>
            {
                new FilterDefinition { Name = "Brand", Field = "Brand" },
                new FilterDefinition { Name = "Category", Field = "Categories" },
                new FilterDefinition { Name = "MinPrice", Field = "MinPrice", SupportRangeFiltering = true }
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
                            Field = fieldDef.Name
                        });
                    }
                }
            }

            return filters;
        }
    }
}